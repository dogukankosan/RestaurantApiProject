using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantWebUI.Dtos.EventDtos;
using System.Net;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminEtkinlik")]
    public class AdminEventController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminEventController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RestaurantApiClient");
            var response = await client.GetAsync("api/Events");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Etkinlikler yüklenemedi.";
                return View(new List<ResultEventDto>());
            }

            var result = await response.Content.ReadFromJsonAsync<List<ResultEventDto>>();
            return View(result ?? new());
        }

        [HttpGet("Ekle")]
        public IActionResult Add()
        {
            return View(new CreateEventDto());
        }

        [HttpPost("Ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateEventDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var client = _httpClientFactory.CreateClient("RestaurantApiClient");
            var response = await client.PostAsJsonAsync("api/Events", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Etkinlik başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                if (problem?.Errors != null)
                {
                    foreach (var kv in problem.Errors)
                        foreach (var msg in kv.Value)
                            ModelState.AddModelError(kv.Key, msg);
                }
            }

            TempData["Type"] = "error";
            TempData["Message"] = "Etkinlik eklenemedi.";
            return View(dto);
        }

        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RestaurantApiClient");
            var response = await client.GetAsync($"api/Events/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Etkinlik bulunamadı.";
                return RedirectToAction("Liste");
            }

            var dto = await response.Content.ReadFromJsonAsync<UpdateEventDto>();
            return View(dto);
        }

        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateEventDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var client = _httpClientFactory.CreateClient("RestaurantApiClient");
            var response = await client.PutAsJsonAsync($"api/Events/{dto.EventID}", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Etkinlik güncellendi.";
                return RedirectToAction("Liste");
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                if (problem?.Errors != null)
                {
                    foreach (var kv in problem.Errors)
                        foreach (var msg in kv.Value)
                            ModelState.AddModelError(kv.Key, msg);
                }
            }

            TempData["Type"] = "error";
            TempData["Message"] = "Etkinlik güncellenemedi.";
            return View(dto);
        }

        [HttpGet("Sil/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RestaurantApiClient");
            var response = await client.DeleteAsync($"api/Events/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Etkinlik silindi.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Etkinlik silinemedi.";
            }

            return RedirectToAction("Liste");
        }

        [HttpPost("DurumGuncelle/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateEventStatusDto dto)
        {
            if (id != dto.EventID)
                return BadRequest(new { message = "ID uyuşmazlığı." });

            var client = _httpClientFactory.CreateClient("RestaurantApiClient");
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/Events/UpdateStatus/{id}")
            {
                Content = JsonContent.Create(dto)
            };
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return NoContent();

            return BadRequest(await response.Content.ReadAsStringAsync());
        }
    }
}