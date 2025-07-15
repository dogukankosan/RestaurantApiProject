using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReservationDtos;
using System.Net;
using System.Text.Json;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminRezervasyon")]
    public class AdminReservationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminReservationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync("api/Reservations");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Rezervasyonlar yüklenemedi.";
                return View(new List<ResultReservationDto>());
            }
            List<ResultReservationDto>? reservations = await response.Content.ReadFromJsonAsync<List<ResultReservationDto>>();
            return View(reservations ?? new());
        }
        [HttpGet("Sil/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.DeleteAsync($"api/Reservations/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Rezervasyon silindi.";
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["Type"] = "warning";
                TempData["Message"] = "Rezervasyon bulunamadı.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Rezervasyon silinirken hata oluştu.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost("DurumGuncelle/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReservationStatusDto dto)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"api/Reservations/UpdateStatus/{id}")
            {
                Content = JsonContent.Create(dto)
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Rezervasyon durumu başarıyla güncellendi." });
            }
            string? content = await response.Content.ReadAsStringAsync();
            return Json(new { success = false, message = content });
        }
        [HttpPost("OkunduToggle/{id}")]
        public async Task<IActionResult> ToggleReadStatus(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpRequestMessage request = new(HttpMethod.Patch, $"api/Reservations/ToggleReadStatus/{id}");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    return Ok(new
                    {
                        success = true,
                        status = result.GetProperty("newStatus").GetBoolean(),
                        message = result.GetProperty("message").GetString()
                    });
                }
                string err = await response.Content.ReadAsStringAsync();
                return BadRequest(new { success = false, message = err });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Durum güncellenirken bir hata oluştu.",
                    detail = ex.Message
                });
            }
        }
    }
}
