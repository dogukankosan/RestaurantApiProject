using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ChefDtos;
using System.Net;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminSef")]
    public class AdminChefController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminChefController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                List<ResultChefDto> chefs = await client.GetFromJsonAsync<List<ResultChefDto>>("api/chefs");
                return View(chefs ?? new List<ResultChefDto>());
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Şefler yüklenemedi.";
                Console.WriteLine($"[Chef-Index] API hatası: {ex.Message}");
                return View(new List<ResultChefDto>());
            }
        }

        [HttpGet("Ekle")]
        public IActionResult Add() => View(new CreateChefDto());

        [HttpPost("Ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateChefDto dto)
        {
            var formFile = Request.Form.Files.GetFile("ChefImageFile");
            if (formFile != null && formFile.Length > 0)
            {
                string[] allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!allowedTypes.Contains(formFile.ContentType))
                    ModelState.AddModelError("ChefImageFile", "Desteklenmeyen görsel türü. Sadece JPG, PNG veya WEBP dosyaları yükleyebilirsiniz.");
                else
                {
                    using MemoryStream ms = new MemoryStream();
                    await formFile.CopyToAsync(ms);
                    dto.ChefImage = ms.ToArray();
                    dto.ChefImageBase64 = $"data:{formFile.ContentType};base64,{Convert.ToBase64String(dto.ChefImage)}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(dto.ChefImageBase64))
            {
                try
                {
                    string base64 = dto.ChefImageBase64.Split(',').Last();
                    dto.ChefImage = Convert.FromBase64String(base64);
                }
                catch
                {
                    ModelState.AddModelError("ChefImageFile", "Önceki yüklenen görsel verisi bozulmuş. Lütfen yeniden yükleyin.");
                }
            }
            else
                ModelState.AddModelError("ChefImageFile", "Şef görseli zorunludur.");
            dto.ChefDescription ??= string.Empty;
            dto.ChefFacebookLink ??= string.Empty;
            dto.ChefInstagramLink ??= string.Empty;
            dto.ChefTwitterLink ??= string.Empty;
            dto.ChefLinkedinLink ??= string.Empty;
            if (!ModelState.IsValid)
                return View(dto);
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/chefs", dto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Şef başarıyla eklendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (problem?.Errors != null)
                    {
                        foreach (var kv in problem.Errors)
                        {
                            string key = kv.Key == "ChefImage" ? "ChefImageFile" : kv.Key;
                            foreach (string msg in kv.Value)
                                ModelState.AddModelError(key, msg);
                        }
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Geçersiz veri gönderildi. Lütfen alanları kontrol edin.");
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Sunucu hatası: {(int)response.StatusCode} {response.StatusCode}\n{content}");
                }
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir (örnek: Serilog, NLog, vs.)
                Console.WriteLine($"[Chef Add] API bağlantı hatası: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Sunucuyla bağlantı kurulamadı. Lütfen daha sonra tekrar deneyin.");
            }
            return View(dto);
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateChefDto? chef = await client.GetFromJsonAsync<UpdateChefDto>($"api/chefs/{id}");
                if (chef == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Şef bulunamadı.";
                    return RedirectToAction("Index");
                }
                if (chef.ChefImage != null && chef.ChefImage.Length > 0)
                    chef.ChefImageBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(chef.ChefImage)}";
                return View(chef);
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Şef bilgisi alınamadı.";
                Console.WriteLine($"[Chef-Update-GET] Hata: {ex.Message}");
                return RedirectToAction("Index");
            }
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateChefDto dto)
        {
            var formFile = Request.Form.Files.GetFile("ChefImageFile");
            if (formFile != null && formFile.Length > 0)
            {
                string[] allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!allowedTypes.Contains(formFile.ContentType))
                    ModelState.AddModelError("ChefImageFile", "Desteklenmeyen görsel türü. Sadece JPG, PNG veya WEBP dosyaları yükleyebilirsiniz.");
                else
                {
                    using MemoryStream ms = new MemoryStream();
                    await formFile.CopyToAsync(ms);
                    dto.ChefImage = ms.ToArray();
                    dto.ChefImageBase64 = $"data:{formFile.ContentType};base64,{Convert.ToBase64String(dto.ChefImage)}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(dto.ChefImageBase64))
            {
                try
                {
                    string base64 = dto.ChefImageBase64.Split(',').Last();
                    dto.ChefImage = Convert.FromBase64String(base64);
                }
                catch
                {
                    ModelState.AddModelError("ChefImageFile", "Önceki yüklenen görsel bozuk. Lütfen yeniden yükleyin.");
                }
            }
            dto.ChefDescription ??= string.Empty;
            dto.ChefFacebookLink ??= string.Empty;
            dto.ChefInstagramLink ??= string.Empty;
            dto.ChefTwitterLink ??= string.Empty;
            dto.ChefLinkedinLink ??= string.Empty;
            if (!ModelState.IsValid)
                return View(dto);
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/chefs/{dto.ChefID}", dto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Şef başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (problem?.Errors != null)
                    {
                        foreach (var kv in problem.Errors)
                        {
                            string key = kv.Key == "ChefImage" ? "ChefImageFile" : kv.Key;
                            foreach (string msg in kv.Value)
                                ModelState.AddModelError(key, msg);
                        }
                    }
                    else
                        ModelState.AddModelError("", "Geçersiz veri gönderildi. Lütfen alanları kontrol edin.");
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Sunucu hatası: {(int)response.StatusCode} - {response.StatusCode}\n{content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Chef-Update] API hatası: {ex.Message}");
                ModelState.AddModelError("", "Sunucuyla bağlantı kurulamadı. Lütfen daha sonra tekrar deneyin.");
            }
            return View(dto);
        }
        [HttpPost("DurumGuncelle/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, bool newStatus)
        {
            UpdateChefStatusDto dto = new UpdateChefStatusDto
            {
                ChefID = id,
                IsActive = newStatus
            };
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"api/chefs/{id}/status")
            {
                Content = JsonContent.Create(dto)
            };
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Şef durumu başarıyla güncellendi.";
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    string errorMessage = problem?.Errors?.SelectMany(e => e.Value).FirstOrDefault() ?? "Geçersiz istek.";
                    TempData["Type"] = "error";
                    TempData["Message"] = $"Hata: {errorMessage}";
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    TempData["Type"] = "error";
                    TempData["Message"] = $"Sunucu hatası: {(int)response.StatusCode} - {response.StatusCode}";
                    Console.WriteLine($"[Chef-StatusUpdate] Sunucu yanıtı: {error}");
                }
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantı hatası oluştu.";
                Console.WriteLine($"[Chef-StatusUpdate] Exception: {ex.Message}");
            }
            return RedirectToAction("Index");
        }

        [HttpPost("Sil/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"api/Chefs/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Şef başarıyla silindi.";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Silinecek şef bulunamadı.";
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    TempData["Type"] = "error";
                    TempData["Message"] = $"Sunucu hatası: {(int)response.StatusCode} - {response.StatusCode}";
                    Console.WriteLine($"[Chef-Delete] API Hatası: {content}");
                }
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantı hatası. Lütfen daha sonra tekrar deneyin.";
                Console.WriteLine($"[Chef-Delete] Exception: {ex.Message}");
            }
            return RedirectToAction("Index");
        }
    }
}