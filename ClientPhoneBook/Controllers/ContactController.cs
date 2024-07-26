using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientPhoneBook.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string endPoint;
        public ContactsController(IHttpClientService httpClientService, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }
        public IActionResult Index(char letter)
        {
            if (letter != '\0')
            {
                ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();
                response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                    ($"{endPoint}Contact/GetAllContacts/" + letter, HttpMethod.Get, HttpContext.Request);


                if (response.Success)
                {
                    return View(response.Data);
                }
            }
            else
            {
                ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();
                response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                    ($"{endPoint}Contact/GetContacts", HttpMethod.Get, HttpContext.Request);


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
                if (contact.file != null && contact.file.Length > 0)
                {
                    var fileName = AddImageFileToPath(contact.file);
                    contact.Image = fileName;
                }
                var apiUrl = $"{endPoint}Contact/UpdateContact";
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
                string apiUrl = $"{endPoint}Contact/AddContact";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, contact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["errorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["errorMesssage"] = "Something went wrong try after some time";
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
        public IActionResult DeleteConfirm(int contactId)
        {
            var apiUrl = $"{endPoint}Contact/DeleteContact/" + contactId;
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
    }
}
