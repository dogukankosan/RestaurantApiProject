using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.IconDtos;
using System.Net;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminIcon")]
    public class AdminIconController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminIconController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync("api/Icons");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "İkonlar alınamadı.";
                return View(new List<ResultIconDto>());
            }
            List<ResultIconDto>? data = await response.Content.ReadFromJsonAsync<List<ResultIconDto>>();
            return View(data ?? new List<ResultIconDto>());
        }
        [HttpGet("Ekle")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("Ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateIconDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Icons", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "İkon başarıyla eklendi.";
                return RedirectToAction("Liste");
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                if (problem?.Errors != null)
                {
                    foreach (var err in problem.Errors)
                        foreach (string msg in err.Value)
                            ModelState.AddModelError(err.Key, msg);
                }
            }
            TempData["Type"] = "error";
            TempData["Message"] = "İkon eklenemedi.";
            return View(dto);
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync($"api/Icons/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "İkon verisi alınamadı.";
                return RedirectToAction("Liste");
            }
            UpdateIconDto dto = await response.Content.ReadFromJsonAsync<UpdateIconDto>();
            return View(dto);
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateIconDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Icons/{dto.IconID}", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "İkon güncellendi.";
                return RedirectToAction("Liste");
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ValidationProblemDetails problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                if (problem?.Errors != null)
                {
                    foreach (var err in problem.Errors)
                        foreach (string msg in err.Value)
                            ModelState.AddModelError(err.Key, msg);
                }
            }
            TempData["Type"] = "error";
            TempData["Message"] = "Güncelleme başarısız.";
            return View(dto);
        }
        [HttpPost("Sil/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.DeleteAsync($"api/Icons/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "İkon silindi.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "İkon silinemedi.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost("DurumGuncelle/{id}")]
        [IgnoreAntiforgeryToken] 
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateIconStatusDto dto)
        {
            if (id != dto.IconID)
            {
                return BadRequest(new { message = "ID eşleşmiyor." });
            }
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/Icons/UpdateStatus/{id}")
            {
                Content = JsonContent.Create(dto)
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return NoContent();
            return BadRequest(await response.Content.ReadAsStringAsync());
        }
    }
}