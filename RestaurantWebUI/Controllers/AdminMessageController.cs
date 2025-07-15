using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.MessageDtos;
using System.Net;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminMesaj")]
    public class AdminMessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminMessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync("api/Messages");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mesajlar yüklenemedi.";
                return View(new List<ResultMessageDto>());
            }
            List<ResultMessageDto>? messages = await response.Content.ReadFromJsonAsync<List<ResultMessageDto>>();
            return View(messages ?? new());
        }
        [HttpGet("Sil/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.DeleteAsync($"api/Messages/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Mesaj başarıyla silindi.";
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["Type"] = "warning";
                TempData["Message"] = "Silinmek istenen mesaj bulunamadı.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mesaj silinirken bir hata oluştu.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost("DurumGuncelle/{id}")]
        public async Task<IActionResult> ToggleReadStatus(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpRequestMessage request = new(HttpMethod.Patch, $"api/Messages/ToggleReadStatus/{id}");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true, message = "Durum güncellendi" });
                }
                string error = await response.Content.ReadAsStringAsync();
                return BadRequest(new { success = false, message = error });
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Durum güncelleme sırasında sunucu hatası oluştu" });
            }
        }
        [HttpPost("TumunuOkunduYap")]
        public async Task<IActionResult> MarkAllAsReadFromAdmin()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, "api/Messages/MarkAllAsRead");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Tüm mesajlar okundu olarak işaretlendi.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Tümünü okundu yap işlemi başarısız.";
            }
            return RedirectToAction("Index");
        }
    }
}