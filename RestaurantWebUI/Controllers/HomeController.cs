using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.WebLogDtos;
using RestaurantWebUI.HelpersMethod;
using System.Text;
using System.Text.Json;

namespace RestaurantWebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            string ip = WebLogHelper.GetUserIp(HttpContext);
            string agent = WebLogHelper.GetUserAgent(HttpContext);
            string location = await WebLogHelper.GetGeoInfoAsync(ip);
            CreateWebLogDto logDto = new CreateWebLogDto
            {
                WebLogUserIPAdress = ip,
                WebLogUserInfo = agent,
                WebLogUserGeo = location,
                WebLogDate = DateTime.Now
            };
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                string? json = JsonSerializer.Serialize(logDto);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/WebLogs", content);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "warning";
                    TempData["Message"] = "Web log gönderilemedi.";
                }
            }
            catch (Exception)
            {

            }
            return View();
        }
    }
}