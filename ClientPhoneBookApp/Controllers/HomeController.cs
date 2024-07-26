using APIPhoneBook.Dto;
using ClientPhoneBookApp.Infrastructure;
using ClientPhoneBookApp.Models;
using ClientPhoneBookApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ClientPhoneBookApp.Controllers
{
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
                ViewModels.ServiceResponse<IEnumerable<ContactViewModel>> response = new ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>();
                response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>>
                    ($"{endPoint}Home/GetAllContacts/" + letter, HttpMethod.Get, HttpContext.Request);


                if (response.Success)
                {
                    return View(response.Data);
                }
            }
            else
            {
                ViewModels.ServiceResponse<IEnumerable<ContactViewModel>> response = new ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>();
                response = _httpClientService.ExecuteApiRequest<ViewModels.ServiceResponse<IEnumerable<ContactViewModel>>>
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
                var serviceResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<ContactViewModel>>(data);
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
                var errorResponse = JsonConvert.DeserializeObject<ViewModels.ServiceResponse<ContactViewModel>>(errorData);
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
