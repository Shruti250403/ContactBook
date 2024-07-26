using APIPhoneBook.Dto;
using ClientPhoneBookApp.Infrastructure;
using ClientPhoneBookApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace ClientPhoneBookApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private string endPoint;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public AuthController(IHttpClientService httpClientService, IConfiguration configuration, IJwtTokenHandler jwtTokenHandler)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _jwtTokenHandler = jwtTokenHandler;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/RegisterUser";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("RegisterSuccess");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try after sometime";
                    }
                }
            }

            return View(viewModel);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ForgetPassword";
                var response = await _httpClientService.PostHttpResponseMessageAsync(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = await response.Content.ReadAsStringAsync();
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                    }
                }
            }
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/LoginUser";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(successResponse);

                    string token = serviceResponse.Data;
                    //Response.Cookies.Append("jwtToken", token, new CookieOptions
                    //{
                    //    HttpOnly = true,
                    //    Secure = true,
                    //    SameSite = SameSiteMode.Strict
                    //});

                    //If you want to access cookie in ajx call then use below code
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });
                    var jwtToken = _jwtTokenHandler.ReadJwtToken(token);
                    var userId = jwtToken.Claims.First(claim => claim.Type == "LoginId").Value;



                    //Get user details
                    var userDetails = UserDetailById(userId);

                    //// Store user image in cookie
                    if (userDetails != null && userDetails.ImageByte != null)
                    {
                        var image = Convert.ToBase64String(userDetails.ImageByte);

                        // Split image into smaller chunks if necessary to fit cookie size limit
                        int chunkSize = 3800; // safe size under 4KB considering other cookie data
                        int totalChunks = (image.Length + chunkSize - 1) / chunkSize;

                        for (int i = 0; i < totalChunks; i++)
                        {
                            string chunk = image.Substring(i * chunkSize, Math.Min(chunkSize, image.Length - i * chunkSize));
                            Response.Cookies.Append($"image_chunk_{i}", chunk, new CookieOptions
                            {
                                HttpOnly = false,
                                Secure = true,
                                SameSite = SameSiteMode.None,
                                Expires = DateTime.UtcNow.AddDays(1),
                            });
                        }
                    }
                    return RedirectToAction("ShowAllContactWithPagination", "Contact");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try after sometime";
                    }
                }
            }
            return View(viewModel);


        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            int chunkIndex = 0;
            while (Request.Cookies.ContainsKey($"image_chunk_{chunkIndex}"))
            {
                Response.Cookies.Delete($"image_chunk_{chunkIndex}");
                chunkIndex++;
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpGet]
        public IActionResult EditUser()
        {
            var username = @User.Identity.Name;
            var apiUrl = $"{endPoint}Auth/GetUserById/" + username;
            var response = _httpClientService.GetHttpResponseMessage<UpdateUserViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<UpdateUserViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    UpdateUserViewModel viewModel = serviceResponse.Data;
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Register");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<UpdateUserViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                }
                return RedirectToAction("Register");
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditUser(UpdateUserViewModel updateContact)
        {
            if (ModelState.IsValid)
            {
                if (updateContact.FormFile != null && updateContact.FormFile.Length > 0)
                {
                    if (updateContact.FormFile.Length > 10240) // 10KB in bytes
                    {

                        TempData["ErrorMessage"] = "Image size should not be greater than 10KB.";
                        return View(updateContact);
                    }

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" }; // Add other allowed extensions as needed
                    var fileExtension = Path.GetExtension(updateContact.FormFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["ErrorMessage"] = "Only JPG, JPEG, and PNG file extensions are allowed.";
                        return View(updateContact);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        updateContact.FormFile.CopyTo(memoryStream);
                        updateContact.ImageByte = memoryStream.ToArray();
                    }
                    updateContact.FileName = updateContact.FormFile.FileName;
                }
                else if (updateContact.RemoveImageHidden == "true")
                {
                    // User wants to remove the current image
                    updateContact.ImageByte = null;
                    updateContact.FileName = null;
                }
                else
                {
                    // Use the previous file name if no new file is provided
                    updateContact.FileName = updateContact.FileName;
                }

                var apiUrl = $"{endPoint}Auth/ModifyUser";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, updateContact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("LogOut");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong try after some time";
                        return RedirectToAction("ShowAllContactWithPagination", "Contact");
                    }
                }
            }
            return View(updateContact);
        }
        [HttpGet]
        [ExcludeFromCodeCoverage]
        public UpdateUserViewModel UserDetailById(string id)
        {

            var apiUrl = $"{endPoint}Auth/GetUserById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateUserViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<UpdateUserViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return serviceResponse.Data;
                }
                else
                {
                    throw new Exception(serviceResponse.Message);

                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<UpdateUserViewModel>>(errorData);
                if (errorResponse != null)
                {
                    throw new Exception(errorResponse.Message);
                }
                else
                {
                    throw new Exception("Something went wrong. Please try after some time.");
                }
            }
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            ForgetViewModel viewModel = new ForgetViewModel();
            viewModel.Username = @User.Identity.Name;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult ChangePassword(ForgetViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ForgetPassword";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request); // Blocking call
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result; // Blocking call
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Logout");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result; // Blocking call
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                    }
                    return RedirectToAction("ChangePassword");
                }
            }
            return View(viewModel);
        }
    }
}
