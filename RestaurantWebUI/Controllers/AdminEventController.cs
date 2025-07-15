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
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync("api/Events");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Etkinlikler yüklenemedi.";
                return View(new List<ResultEventDto>());
            }
            List<ResultEventDto> result = await response.Content.ReadFromJsonAsync<List<ResultEventDto>>();
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
            if (dto.EventImageFile != null && dto.EventImageFile.Length > 0)
            {
                using MemoryStream ms = new MemoryStream();
                await dto.EventImageFile.CopyToAsync(ms);
                dto.EventImage = ms.ToArray();
                dto.EventImageBase64 = $"data:{dto.EventImageFile.ContentType};base64,{Convert.ToBase64String(dto.EventImage)}";
            }
            else if (!string.IsNullOrWhiteSpace(dto.EventImageBase64))
            {
                try
                {
                    string base64Data = dto.EventImageBase64.Split(',').Last();
                    dto.EventImage = Convert.FromBase64String(base64Data);
                }
                catch
                {
                    ModelState.AddModelError("EventImageFile", "Önceki yüklenen görsel okunamadı, lütfen yeniden yükleyin.");
                }
            }
            else
                ModelState.AddModelError("EventImageFile", "Etkinlik görseli zorunludur.");
            if (!ModelState.IsValid)
                return View(dto);
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Events", dto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Etkinlik başarıyla eklendi.";
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
                else
                    ModelState.AddModelError(string.Empty, "Etkinlik ekleme başarısız oldu.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Sunucuyla iletişimde hata oluştu.");
            }
            return View(dto);
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync($"api/Events/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Etkinlik bulunamadı.";
                return RedirectToAction("Index");
            }
            UpdateEventDto? dto = await response.Content.ReadFromJsonAsync<UpdateEventDto>();
            if (dto?.EventImage is not null && dto.EventImage.Length > 0)
                dto.EventImageBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(dto.EventImage)}";
            return View(dto);
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateEventDto dto)
        {
            if (dto.EventImageFile != null && dto.EventImageFile.Length > 0)
            {
                using MemoryStream ms = new MemoryStream();
                await dto.EventImageFile.CopyToAsync(ms);
                dto.EventImage = ms.ToArray();
                dto.EventImageBase64 = $"data:{dto.EventImageFile.ContentType};base64,{Convert.ToBase64String(dto.EventImage)}";
            }
            else if (!string.IsNullOrWhiteSpace(dto.EventImageBase64))
            {
                try
                {
                    string base64Data = dto.EventImageBase64.Split(',').Last();
                    dto.EventImage = Convert.FromBase64String(base64Data);
                }
                catch
                {
                    ModelState.AddModelError("EventImageFile", "Önceki yüklenen görsel okunamadı, lütfen yeniden yükleyin.");
                }
            }
            if (!ModelState.IsValid)
                return View(dto);
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Events/{dto.EventID}", dto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Etkinlik başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (problem?.Errors is not null)
                    {
                        foreach (var kv in problem.Errors)
                            foreach (string msg in kv.Value)
                                ModelState.AddModelError(kv.Key, msg);
                    }
                }
                else
                    ModelState.AddModelError(string.Empty, "Etkinlik güncelleme sırasında hata oluştu.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Sunucuyla iletişim kurulamadı.");
            }
            return View(dto);
        }
        [HttpGet("Sil/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.DeleteAsync($"api/Events/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Etkinlik başarıyla silindi.";
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["Type"] = "warning";
                TempData["Message"] = "Silinmek istenen etkinlik bulunamadı.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Etkinlik silinirken bir hata oluştu.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost("DurumGuncelle/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateEventStatusDto dto)
        {
            if (id != dto.EventID)
                return BadRequest(new { message = "ID uyuşmazlığı. Lütfen sayfayı yenileyin." });
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"api/Events/UpdateStatus/{id}")
                {
                    Content = JsonContent.Create(dto)
                };
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return NoContent(); 
                string errorMessage = await response.Content.ReadAsStringAsync();
                return BadRequest(new { message = errorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Durum güncellenirken bir hata oluştu." });
            }
        }
    }
}