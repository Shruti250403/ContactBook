using ClientContactPhonebook.Infrastructure;
using ClientContactPhonebook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientContactPhonebook.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
        {
            private readonly IHttpClientService _httpClientService;
            private readonly IConfiguration _configuration;
            private string endPoint;
            public AuthController(IHttpClientService httpClientService, IConfiguration configuration)
            {
                _httpClientService = httpClientService;
                _configuration = configuration;
                endPoint = _configuration["EndPoint:CivicaApi"];
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
                    var apiUrl = $"{endPoint}Auth/Register";
                    var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                    if (response.IsSuccessStatusCode)
                    {
                        string successResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<APIPhoneBook.Dto.ServiceResponse<string>>(successResponse);
                        TempData["SuccessMessage"] = serviceResponse.Message;
                        return RedirectToAction("RegisterSuccess");
                    }
                    else
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                        if (errorResponse != null)
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
            public IActionResult Login()
            {
                return View();
            }
            [HttpPost]
            public IActionResult Login(LoginViewModel viewModel)
            {
                if (ModelState.IsValid)
                {
                    var apiUrl = $"{endPoint}Auth/Login";
                    var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                    if (response.IsSuccessStatusCode)
                    {
                        string successResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

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
                        TempData["SuccessMessage"] = serviceResponse.Message;
                        return RedirectToAction("Index", "Contact");
                    }
                    else
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().Result;
                        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                        if (errorResponse != null)
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

                return RedirectToAction("Index", "Home");
            }
        }
}
