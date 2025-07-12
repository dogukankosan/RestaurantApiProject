using Microsoft.AspNetCore.Mvc;
using System.Net;
using RestaurantWebUI.Dtos.FeatureDtos;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminOzellik")]
    public class AdminFeatureController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminFeatureController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/Features");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Özellik bilgileri alınamadı.";
                    return View();
                }
                UpdateFeatureDto result = await response.Content.ReadFromJsonAsync<UpdateFeatureDto>();
                if (result == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Özellik bilgisi boş döndü.";
                    return View();
                }
                if (result.FeatureImageData is { Length: > 0 })
                {
                    string mimeType = "image/jpeg"; 
                    string base64 = Convert.ToBase64String(result.FeatureImageData);
                    result.FeatureImageBase64 = $"data:{mimeType};base64,{base64}";
                }
                return View(result);
            }
            catch
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API ile bağlantı kurulamadı.";
                return View();
            }
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/Features");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Özellik verisi alınamadı.";
                    return RedirectToAction("Index");
                }
                UpdateFeatureDto featureData = await response.Content.ReadFromJsonAsync<UpdateFeatureDto>();
                if (featureData == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Özellik verisi boş döndü.";
                    return RedirectToAction("Index");
                }
                if (featureData.FeatureImageData is { Length: > 0 })
                {
                    string mimeType = "image/jpeg";
                    string base64 = Convert.ToBase64String(featureData.FeatureImageData);
                    featureData.FeatureImageBase64 = $"data:{mimeType};base64,{base64}";
                }
                return View(featureData);
            }
            catch
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantısı sağlanamadı.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateFeatureDto dto)
        {
            if (dto.FeatureImageFile != null && dto.FeatureImageFile.Length > 0)
            {
                using MemoryStream ms = new();
                await dto.FeatureImageFile.CopyToAsync(ms);
                dto.FeatureImageData = ms.ToArray();
            }
            else if (!string.IsNullOrWhiteSpace(dto.FeatureImageBase64))
            {
                try
                {
                    string base64Data = dto.FeatureImageBase64.Split(',').Last();
                    dto.FeatureImageData = Convert.FromBase64String(base64Data);
                }
                catch
                {
                    ModelState.AddModelError("FeatureImageFile", "Görsel base64 verisi okunamadı, lütfen tekrar yükleyin.");
                }
            }
            else
                ModelState.AddModelError("FeatureImageFile", "Görsel zorunludur.");
            if (!ModelState.IsValid)
                return View(dto); 
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("api/Features", dto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Özellik başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (problem?.Errors != null)
                    {
                        foreach (var kvp in problem.Errors)
                        {
                            foreach (string msg in kvp.Value)
                                ModelState.AddModelError(kvp.Key, msg);
                        }
                    }
                    else
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"Geçersiz veri: {content}");
                    }
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Sunucu hatası: {(int)response.StatusCode} - {response.StatusCode}\n{content}");
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Sunucuya bağlanılamadı.");
            }
            return View(dto);
        }
    }
}