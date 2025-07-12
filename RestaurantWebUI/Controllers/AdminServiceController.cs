using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantWebUI.Dtos.IconDtos;
using RestaurantWebUI.Dtos.ServiceDtos;
using System.Net;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminServis")]
    public class AdminServiceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminServiceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync("api/Services");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Servisler yüklenemedi.";
                return View(new List<ResultServiceDto>());
            }
            List<ResultServiceDto>? result = await response.Content.ReadFromJsonAsync<List<ResultServiceDto>>();
            return View(result ?? new List<ResultServiceDto>());
        }
        [HttpGet("Ekle")]
        public async Task<IActionResult> Add()
        {
            CreateServiceDto dto = new CreateServiceDto();
            await PopulateIconOptionsAsync(dto);
            return View(dto);
        }
        [HttpPost("Ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateServiceDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateIconOptionsAsync(dto);
                return View(dto);
            }
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Services", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Servis başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                if (problem?.Errors != null)
                {
                    foreach (var kv in problem.Errors)
                        foreach (string msg in kv.Value)
                            ModelState.AddModelError(kv.Key, msg);
                }
            }
            TempData["Type"] = "error";
            TempData["Message"] = "Servis eklenemedi.";
            await PopulateIconOptionsAsync(dto);
            return View(dto);
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync($"api/Services/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Servis bulunamadı.";
                return RedirectToAction("Liste");
            }
            UpdateServiceDto? dto = await response.Content.ReadFromJsonAsync<UpdateServiceDto>();
            if (dto is null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Veri çözümlenemedi.";
                return RedirectToAction("Liste");
            }
            await PopulateIconOptionsAsync(dto);
            return View(dto);
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateServiceDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateIconOptionsAsync(dto);
                return View(dto);
            }
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Services/{dto.ServiceID}", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Servis güncellendi.";
                return RedirectToAction("Index");
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ValidationProblemDetails problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                if (problem?.Errors != null)
                {
                    foreach (var kv in problem.Errors)
                        foreach (string msg in kv.Value)
                            ModelState.AddModelError(kv.Key, msg);
                }
            }
            TempData["Type"] = "error";
            TempData["Message"] = "Servis güncellenemedi.";
            await PopulateIconOptionsAsync(dto);
            return View(dto);
        }
        [HttpGet("Sil/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.DeleteAsync($"api/Services/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Servis silindi.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Servis silinemedi.";
            }
            return RedirectToAction("Index");
        }
        private async Task PopulateIconOptionsAsync(CreateServiceDto dto)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync("api/Icons");
            if (response.IsSuccessStatusCode)
            {
                List<ResultIconDto>? icons = await response.Content.ReadFromJsonAsync<List<ResultIconDto>>();
                dto.IconOptions = icons?
                    .Where(x => x.IconStatus)
                    .Select(x => new SelectListItem
                    {
                        Text = x.IconURL,
                        Value = x.IconURL
                    }).ToList() ?? new();
            }
            else
                dto.IconOptions = new();
        }
        [HttpPost("DurumGuncelle/{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateServiceStatusDto dto)
        {
            if (id != dto.ServiceID)
            {
                return BadRequest(new { message = "ID eşleşmiyor." });
            }
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/Services/UpdateStatus/{id}")
            {
                Content = JsonContent.Create(dto)
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return NoContent(); 
            string content = await response.Content.ReadAsStringAsync();
            return BadRequest(content);
        }
    }
}
