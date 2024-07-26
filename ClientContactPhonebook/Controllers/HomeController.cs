using APIPhoneBook.Dto;
using ClientContactPhonebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClientContactPhonebook.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private string endPoint;
        public HomeController(IHttpClientService httpClientService, IConfiguration configuration, ILogger<HomeController> logger)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _logger = logger;
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
        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Home/GetContactById/" + id;
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


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
