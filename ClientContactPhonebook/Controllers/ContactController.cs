using APIPhoneBook.Dto;
using ClientContactPhonebook.Infrastructure;
using ClientContactPhonebook.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientContactPhonebook.Controllers
{
    public class ContactController
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string endPoint;
        public ContactController(IHttpClientService httpClientService, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index(char? letter)
        {
            if (letter != null)
            {
                ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();
                response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                    ($"{endPoint}Home/GetAllContacts/" + letter, HttpMethod.Get, HttpContext.Request);


                if (response.Success)
                {
                    return View(response.Data);
                }
            }
            else
            {
                ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();
                response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                    ($"{endPoint}Home/GetContacts", HttpMethod.Get, HttpContext.Request);


                if (response.Success)
                {
                    return View(response.Data);
                }
            }
            return View(new List<ContactViewModel>());
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]

        public IActionResult Edit(UpdateContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                if (contact.Image != null && contact.Image.Length > 0)
                {
                    var fileName = AddImageFileToPath(contact.Image);
                    contact.Image = fileName;
                }
                var apiUrl = $"{endPoint}Contact/ModifyContact";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, contact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
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
                        TempData["ErrorMessage"] = "Something went wrong. Please try after sometime.";
                    }
                }
            }


            return View(contact);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                if (contact.file != null && contact.file.Length > 0)
                {
                    var fileName = AddImageFileToPath(contact.file);
                    contact.Image = fileName;
                }
                string apiUrl = $"{endPoint}Contact/Create";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, contact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMesssage"] = "Something went wrong try after some time";
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction();
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
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                }
                return RedirectToAction("Index");
            }

        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Someething went wrong.Try again later.";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int phoneId)
        {
            var apiUrl = $"{endPoint}Contact/Remove/" + phoneId;
            var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>
                ($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");

            }
            else
            {

                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Index");

            }
        }
        private string AddImageFileToPath(IFormFile imageFile)

        {

            // Process the uploaded file(eq. save it to disk)
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");

            var filePath = Path.Combine(uploadsFolder, imageFile.FileName);

            // Save the file to storage and set path

            using (var stream = new FileStream(filePath, FileMode.Create))

            {

                imageFile.CopyTo(stream);

                return imageFile.FileName;

            }
        }
        [HttpGet]
        public IActionResult ShowAllContactWithPagination(int page = 1, int pageSize = 2)
        {
            var apiGetPositionsUrl = $"{endPoint}Contact/GetAllContactsByPagination" + "?page=" + page + "&pageSize=" + pageSize;


            var apiGetCountUrl = $"{endPoint}Contact/GetContactsCount";

            ServiceResponse<int> countOfContacts = new ServiceResponse<int>();

            countOfContacts = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalCount = countOfContacts.Data;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (page > totalPages)
            {
                return RedirectToAction("ShowAllContactWithPagination", new { page = totalPages, pageSize });
            }

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;


            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetPositionsUrl, HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }
    }
}
