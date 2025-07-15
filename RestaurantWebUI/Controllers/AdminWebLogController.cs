using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.WebLogDtos;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminSiteyeGiren")]
    public class AdminWebLogController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminWebLogController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync("api/WebLogs");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Web log verileri alınamadı.";
                return View(new List<ResultWebLogDto>());
            }
            List<ResultWebLogDto>? data = await response.Content.ReadFromJsonAsync<List<ResultWebLogDto>>();
            return View(data ?? new List<ResultWebLogDto>());
        }
        [HttpPost("Sil/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.DeleteAsync($"api/WebLogs/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Web log kaydı silindi.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Web log silinemedi.";
            }
            return RedirectToAction("Index");
        }
    }
}