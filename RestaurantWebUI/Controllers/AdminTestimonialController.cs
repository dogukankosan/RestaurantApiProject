using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.TestimonialDtos;
using System.Net;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminReferans")]
    public class AdminTestimonialController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminTestimonialController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/Testimonials");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Referanslar alınamadı.";
                    return View(new List<ResultTestimonialDto>());
                }
                List<ResultTestimonialDto>? data = await response.Content.ReadFromJsonAsync<List<ResultTestimonialDto>>();
                return View(data ?? new List<ResultTestimonialDto>());
            }
            catch
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantı hatası.";
                return View(new List<ResultTestimonialDto>());
            }
        }
        [HttpGet("Ekle")]
        public IActionResult Add()
        {
            return View(new CreateTestimonialDto());
        }
        [HttpPost("Ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateTestimonialDto dto)
        {
            if (dto.TestimonialImageFile != null && dto.TestimonialImageFile.Length > 0)
            {
                using MemoryStream ms = new();
                await dto.TestimonialImageFile.CopyToAsync(ms);
                dto.TestimonialImage = ms.ToArray(); 
                dto.TestimonialImageBase64 = $"data:{dto.TestimonialImageFile.ContentType};base64,{Convert.ToBase64String(dto.TestimonialImage)}"; 
            }
            else if (!string.IsNullOrWhiteSpace(dto.TestimonialImageBase64))
            {
                try
                {
                    string base64 = dto.TestimonialImageBase64.Split(',').Last();
                    dto.TestimonialImage = Convert.FromBase64String(base64); 
                }
                catch
                {
                    ModelState.AddModelError("TestimonialImageFile", "Base64 veri okunamadı.");
                }
            }
            else
                ModelState.AddModelError("TestimonialImageFile", "Görsel zorunludur.");
            if (!ModelState.IsValid)
                return View(dto);
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Testimonials", new
            {
                dto.TestimonialNameSurname,
                dto.TestimonialTitle,
                dto.TestimonialComment,
                dto.TestimonialStatus,
                dto.TestimonialImage
            });
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Referans başarıyla eklendi.";
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
            TempData["Message"] = "Referans eklenemedi.";
            return View(dto);
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.GetAsync($"api/Testimonials/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Referans bulunamadı.";
                return RedirectToAction("Index");
            }
            UpdateTestimonialDto? dto = await response.Content.ReadFromJsonAsync<UpdateTestimonialDto>();
            if (dto == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Veri çözümlenemedi.";
                return RedirectToAction("Index");
            }
            if (dto.TestimonialImage != null && dto.TestimonialImage.Length > 0)
            {
                dto.TestimonialImageBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(dto.TestimonialImage)}";
            }
            return View(dto);
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTestimonialDto dto)
        {
            if (dto.TestimonialImageFile != null && dto.TestimonialImageFile.Length > 0)
            {
                using MemoryStream ms = new();
                await dto.TestimonialImageFile.CopyToAsync(ms);
                dto.TestimonialImage = ms.ToArray();
                dto.TestimonialImageBase64 = $"data:{dto.TestimonialImageFile.ContentType};base64,{Convert.ToBase64String(dto.TestimonialImage)}";
            }
            else if (!string.IsNullOrWhiteSpace(dto.TestimonialImageBase64))
            {
                try
                {
                    string base64 = dto.TestimonialImageBase64.Split(',').Last();
                    dto.TestimonialImage = Convert.FromBase64String(base64);
                }
                catch
                {
                    ModelState.AddModelError("TestimonialImageFile", "Base64 görsel okunamadı.");
                }
            }
            else
                ModelState.AddModelError("TestimonialImageFile", "Görsel zorunludur.");
            if (!ModelState.IsValid)
                return View(dto);
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Testimonials/{dto.TestimonialID}", new
            {
                dto.TestimonialID,
                dto.TestimonialNameSurname,
                dto.TestimonialTitle,
                dto.TestimonialComment,
                dto.TestimonialStatus,
                dto.TestimonialImage
            });
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Referans başarıyla güncellendi.";
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
            TempData["Message"] = "Referans güncellenemedi.";
            return View(dto);
        }
        [HttpGet("Sil/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpResponseMessage response = await client.DeleteAsync($"api/Testimonials/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Referans başarıyla silindi.";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Silinmek istenen referans bulunamadı.";
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Type"] = "error";
                    TempData["Message"] = $"Silme işlemi başarısız oldu. Sunucu cevabı: {(int)response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Sunucuya ulaşılamadı. Hata: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
        [HttpPost("DurumGuncelle/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, bool newStatus)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            UpdateTestimonialStatusDto dto = new UpdateTestimonialStatusDto()
            {
                TestimonialID = id,
                IsActive = newStatus
            };
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/Testimonials/UpdateStatus/{id}")
            {
                Content = JsonContent.Create(dto)
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Durum başarıyla güncellendi.";
            }
            else
            {
                string detail = await response.Content.ReadAsStringAsync();
                TempData["Type"] = "error";
                TempData["Message"] = $"Durum güncellenemedi: {detail}";
            }
            return RedirectToAction("Index");
        }
    }
}
