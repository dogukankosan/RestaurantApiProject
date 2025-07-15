using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReportDtos;

namespace RestaurantWebUI.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }   
        [Route("Anasayfa")]
        public async Task<IActionResult> Home()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                ResultAdminReportDto? report = await client.GetFromJsonAsync<ResultAdminReportDto>("api/Reports/admin");
                if (report == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Dashboard verileri alınamadı.";
                    return View(new ResultAdminReportDto());
                }
                return View(report);
            }
            catch (Exception)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantısı sırasında hata oluştu.";
                return View(new ResultAdminReportDto());
            }
        }
    }
}