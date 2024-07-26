using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using ClientPhoneBookApp.Infrastructure;
using ClientPhoneBookApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Policy;

namespace ClientPhoneBookApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IImageUpload _imageUpload;
        private string endPoint;
        public ContactController(IHttpClientService httpClientService, IConfiguration configuration, IImageUpload imageUpload)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _imageUpload = imageUpload;
        }
        public IActionResult ShowAllContactWithPagination(char? letter, string? search, int page = 1, int pageSize = 2,string sortOrder="asc")
        {
            var apiGetContactsUrl = "";
            var apiGetCountUrl = "";
            var apiGetLettersUrl = $"{endPoint}Contact/GetAllContacts";
            if (letter != null || search!=null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination?letter={letter}&search={search}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount?letter={letter}&search={search}";
            }
            
            else
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination?page=" + page + "&pageSize=" + pageSize + "&sortOrder=" + sortOrder;
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount";
            }
            // Fetch the total count of contacts
            var countOfContact = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, HttpContext.Request);
            if (countOfContact == null || !countOfContact.Success)
            {
                return View(new List<ContactViewModel>());
            }
            int totalCount = countOfContact.Data;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Letter = letter;
            ViewBag.search = search;
            ViewBag.SortOrder = sortOrder;
            if (totalCount == 0)
            {
                return View(new List<ContactViewModel>());
            }
            if (page > totalPages)
            {
                // Redirect to the first page with the new page size
                return RedirectToAction("ShowAllContactWithPagination", new { letter, page = 1, pageSize, search,sortOrder });
            }
            ViewModels.ServiceResponse<IEnumerable<ContactViewModel>> response = new ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>();
                response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>>
                    (apiGetContactsUrl, HttpMethod.Get, HttpContext.Request);
            ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>? getLetters = new ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>();

            getLetters = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetLettersUrl, HttpMethod.Get, HttpContext.Request);

            if (getLetters.Success)
            {
                var distinctLetters = getLetters.Data.Select(contact => char.ToUpper(contact.FirstName.FirstOrDefault()))
                                                            .Where(firstLetter => firstLetter != default(char))
                                                            .Distinct()
                                                             .OrderBy(letter => letter)
                                                            .ToList();
                ViewBag.DistinctLetters = distinctLetters;

            }
            if (response != null && response.Success)
            {
                    return View(response.Data);
                }
            return View(new List<ContactViewModel>());
        }
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<UpdateContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    UpdateContactViewModel viewModel = serviceResponse.Data;
                    viewModel.Countries = GetCountry();
                    viewModel.States = GetState();
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<UpdateContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }
                return RedirectToAction("ShowAllContactWithPagination");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(UpdateContactViewModel contact)
        {
            contact.Countries = GetCountry();
            contact.States = GetState();
            if (ModelState.IsValid)
            {
                if (contact.file != null && contact.file.Length > 0)
                {
                    var fileName = Path.GetFileName(contact.file.FileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
                        return View(contact);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        contact.file.CopyTo(memoryStream);
                        contact.ImageByte = memoryStream.ToArray();
                    }

                    fileName = _imageUpload.AddImageFileToPath(contact.file);
                    contact.Image = fileName;
                }
                else if (contact.RemoveImage)
                {
                    contact.Image = null; // Set FileName to null to remove the image
                    contact.ImageByte = null;
                }
                else
                {
                    contact.Image = contact.Image;
                }
                var apiUrl = $"{endPoint}Contact/ModifyContact";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, contact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse?.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse?.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try after sometime.";
                    }
                }
            }


            return View(contact);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            AddContactViewModel viewModel = new AddContactViewModel();
            viewModel.States = GetState();
            viewModel.Countries = GetCountry();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AddContactViewModel contact)
        {
            contact.Countries = GetCountry();
            contact.States = GetState();
            if (ModelState.IsValid)
            {
                if (contact.file != null && contact.file.Length > 0)
                {
                    var fileName = Path.GetFileName(contact.file.FileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
                        return View(contact);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        contact.file.CopyTo(memoryStream);
                        contact.ImageByte = memoryStream.ToArray();
                    }

                    fileName = _imageUpload.AddImageFileToPath(contact.file);
                    contact.Image = fileName;
                }
                string apiUrl = $"{endPoint}Contact/Create";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, contact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse?.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse?.Message;
                    }
                    else
                    {
                        TempData["ErrorMesssage"] = "Something went wrong try after some time";
                        
                    }
                }
                return RedirectToAction("ShowAllContactWithPagination");
            }

            return View(contact);
        }

        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<ContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse?.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                }
                return RedirectToAction("ShowAllContactWithPagination");
            }

        }


        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<ContactViewModel>>(data);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Someething went wrong.Try again later.";
                }
                return RedirectToAction("ShowAllContactWithPagination");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteConfirm(int contactId)
        {
            var apiUrl = $"{endPoint}Contact/Remove/" + contactId;
            var response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<string>>
                ($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("ShowAllContactWithPagination");

            }
            else
            {

                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("ShowAllContactWithPagination");

            }
        }
        //[ExcludeFromCodeCoverage]
        //private string AddImageFileToPath(IFormFile imageFile)

        //{

        //    // Process the uploaded file(eq. save it to disk)
        //    var uploadsFolder = Path.Combine(_imageUpload.WebRootPath, "Uploads");

        //    var filePath = Path.Combine(uploadsFolder, imageFile.FileName);

        //    // Save the file to storage and set path

        //    using (var stream = new FileStream(filePath, FileMode.Create))

        //    {

        //        imageFile.CopyTo(stream);

        //        return imageFile.FileName;

        //    }
        //}
        [ExcludeFromCodeCoverage]
        private List<StateViewModel> GetState()
        {
            ViewModels.ServiceResponse<IEnumerable<StateViewModel>> response = new ViewModels.ServiceResponse<IEnumerable<StateViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<StateViewModel>>>
                ($"{endPoint}State/GetStates", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<StateViewModel>();
        }
        [ExcludeFromCodeCoverage]
        private List<CountryViewModel> GetCountry()
        {
            ViewModels.ServiceResponse<IEnumerable<CountryViewModel>> response = new ViewModels.ServiceResponse<IEnumerable<CountryViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<CountryViewModel>>>
                ($"{endPoint}Country/GetAll", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<CountryViewModel>();
        }
        public IActionResult ShowAllContactWithPaginationFav(char? letter, int page = 1, int pageSize = 2, string sortOrder = "asc")
        {
            var apiGetContactsUrl = "";
            var apiGetCountUrl = "";
            var apiGetLettersUrl = $"{endPoint}Contact/GetAllFavouriteContacts";

            
            if (letter != null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/favourites/?letter={letter}&page={page}&pageSize={pageSize}";
                
                apiGetCountUrl = $"{endPoint}Contact/GetTotalCountOfFavContacts/?letter={letter}";
            }
            else
            {
                apiGetContactsUrl = $"{endPoint}Contact/favourites?page=" + page + "&pageSize=" + pageSize ;
                apiGetCountUrl = $"{endPoint}Contact/GetTotalCountOfFavContacts";

            }
            ViewModels.ServiceResponse<int> countOfContact = new ViewModels.ServiceResponse<int>();

            countOfContact = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);
            if (countOfContact == null || !countOfContact.Success)
            {
                return View(new List<ContactViewModel>());
            }
            int totalCount = countOfContact.Data;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Letter = letter;

            if (totalCount == 0)
            {
                // Return an empty view
                return View(new List<ContactViewModel>());
            }


            if (page > totalPages)
            {
                // Redirect to the first page with the new page size
                return RedirectToAction("ShowAllContactWithPaginationFav", new { letter, page = 1, pageSize });
            }


            ViewModels.ServiceResponse<IEnumerable<ContactViewModel>> response = new ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetContactsUrl, HttpMethod.Get, HttpContext.Request);
            ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>? getLetters = new ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>();

            getLetters = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetLettersUrl, HttpMethod.Get, HttpContext.Request);

            if (getLetters.Success)
            {
                var distinctLetters = getLetters.Data.Select(contact => char.ToUpper(contact.FirstName.FirstOrDefault()))
                                                            .Where(firstLetter => firstLetter != default(char))
                                                            .Distinct()
                                                             .OrderBy(letter => letter)
                                                            .ToList();
                ViewBag.DistinctLetters = distinctLetters;

            }

            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }
        public IActionResult ContactReports()
        {

            var countries = GetCountry();
            var states = GetState();

            ViewBag.Countries = countries;
            ViewBag.States = states;
            return PartialView("_ReportPartialView");
        }

        public IActionResult GetAllContactsCountByCountrySP(int country)
        {
            var apiGetCountUrl = "";

            ViewModels.ServiceResponse<int> count_response = new ViewModels.ServiceResponse<int>();

            if (country >= 0)
            {

                apiGetCountUrl = $"{endPoint}Contact/GetContactsCountByCountrySP?countryId={country}";
            }
            else
            {
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCountByCountrySP";
            }


            count_response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalContactsCount = count_response.Data;


            var countries = GetCountry();
            var states = GetState();

            ViewBag.Countries = countries;
            ViewBag.States = states;

            ViewBag.Country = country;
            ViewBag.TotalCount = totalContactsCount;




            if (totalContactsCount == 0)
            {
                return View(new List<CountViewModel>());
            }
            else
            {
                if (count_response.Success)
                {
                    return View(new List<CountViewModel>());

                }
            }

            return View(new List<CountViewModel>());
        }

        public IActionResult GetAllContactsByStatesSP(int state)
        {
            var apiGetUrl = "";

            ViewModels.ServiceResponse<IEnumerable<ContactSPViewModel>> response = new ViewModels.ServiceResponse<IEnumerable<ContactSPViewModel>>();

            if (state >= 0)
            {

                apiGetUrl = $"{endPoint}Contact/GetContactsByStateSP?stateId={state}";
            }
            else
            {
                apiGetUrl = $"{endPoint}Contact/GetContactsByStateSP";
            }


            response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse< IEnumerable<ContactSPViewModel>>> (apiGetUrl, HttpMethod.Get, HttpContext.Request);



            var countries = GetCountry();
            var states = GetState();

            ViewBag.Countries = countries;
            ViewBag.States = states;

            ViewBag.State = state;


            
                if (response.Success)
                {
                    return View(response.Data.ToList());

                }

            return View(new List<ContactSPViewModel>());
        }

        public IActionResult GetAllContactsCountByGenderSP(string gender)
        {
            var apiGetCountUrl = "";
            if (gender == null)
            {
                gender = "null";
            }

            ViewModels.ServiceResponse<int> count_response = new ViewModels.ServiceResponse<int>();

            if (gender != null)
            {

                apiGetCountUrl = $"{endPoint}Contact/GetContactCountByGenderSP?gender={gender}";
            }
            else
            {
                apiGetCountUrl = $"{endPoint}Contact/GetContactCountByGenderSP";
            }


            count_response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalContactsCount = count_response.Data;

            var countries = GetCountry();
            var states = GetState();

            ViewBag.Countries = countries;
            ViewBag.States = states;

            ViewBag.Gender = gender;
            ViewBag.TotalCount = totalContactsCount;




            if (totalContactsCount == 0)
            {
                return View(new List<CountViewModel>());
            }
            else
            {
                if (count_response.Success)
                {

                    return View(new List<CountViewModel>());

                }
            }

            return View(new List<CountViewModel>());
        }

        public IActionResult GetAllContactsByBirthdayMonthSP(int month)
        {
            var apiGetUrl = "";

            ViewModels.ServiceResponse<IEnumerable<ContactSPViewModel>> response = new ViewModels.ServiceResponse<IEnumerable<ContactSPViewModel>>();

            if (month >= 0)
            {

                apiGetUrl = $"{endPoint}Contact/GetContactsByBirthMonthSP?month={month}";
            }
            else
            {
                apiGetUrl = $"{endPoint}Contact/GetContactsByBirthMonthSP";
            }


            response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<ContactSPViewModel>>>(apiGetUrl, HttpMethod.Get, HttpContext.Request);



            var countries = GetCountry();
            var states = GetState();

            ViewBag.Countries = countries;
            ViewBag.States = states;

            ViewBag.Month = month;


           
                if (response.Success)
                {
                    return View(response.Data.ToList());

                }

            return View(new List<ContactSPViewModel>());
        }
    }
}
