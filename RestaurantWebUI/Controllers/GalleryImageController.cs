using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.GalleryImageDtos;
using System.Net;
using System.Text.Json;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminGaleri")]
    public class GalleryImageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GalleryImageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                List<ResultGalleryImageDto> images = await client.GetFromJsonAsync<List<ResultGalleryImageDto>>("api/galleryimages");
                return View(images ?? new List<ResultGalleryImageDto>());
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Galeri verileri yüklenemedi.";
                Console.WriteLine($"[Gallery-Index] API Hatası: {ex.Message}");
                return View(new List<ResultGalleryImageDto>());
            }
        }

        [HttpGet("Ekle")]
        public IActionResult Add() => View(new CreateGalleryImageDto());

        [HttpPost("Ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateGalleryImageDto dto)
        {
            try
            {
                if (dto.ImageFile is { Length: > 0 })
                {
                    string[] allowedTypes = { "image/jpeg", "image/png", "image/webp" };
                    long maxSizeInBytes = 5 * 1024 * 1024; 
                    if (!allowedTypes.Contains(dto.ImageFile.ContentType))
                        ModelState.AddModelError(nameof(dto.ImageFile), "Desteklenmeyen dosya türü. (Yalnızca .jpg, .png, .webp)");
                    else if (dto.ImageFile.Length > maxSizeInBytes)
                        ModelState.AddModelError(nameof(dto.ImageFile), "Görsel dosyası 5 MB'dan büyük olamaz.");
                    else
                    {
                        await using MemoryStream ms = new MemoryStream();
                        await dto.ImageFile.CopyToAsync(ms);
                        dto.ImageByte = ms.ToArray();
                        dto.ImageBase64 = $"data:{dto.ImageFile.ContentType};base64,{Convert.ToBase64String(dto.ImageByte)}";
                    }
                }
                else if (!string.IsNullOrEmpty(dto.ImageBase64))
                {
                    string base64Data = dto.ImageBase64.Split(',').Last();
                    try
                    {
                        dto.ImageByte = Convert.FromBase64String(base64Data);
                    }
                    catch (FormatException)
                    {
                        ModelState.AddModelError(nameof(dto.ImageFile), "Base64 görsel verisi çözümlenemedi.");
                    }
                }
                else
                    ModelState.AddModelError(nameof(dto.ImageFile), "Görsel yüklemek zorunludur.");
                if (!ModelState.IsValid)
                    return View(dto);
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/GalleryImages", dto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Görsel başarıyla eklendi.";
                    return RedirectToAction("Index");
                }
                string errorContent = await response.Content.ReadAsStringAsync();
                try
                {
                    ValidationProblemDetails? problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(errorContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (problemDetails?.Errors != null)
                    {
                        foreach (var error in problemDetails.Errors)
                        {
                            foreach (string message in error.Value)
                                ModelState.AddModelError(error.Key, message);
                        }
                    }
                    else
                        ModelState.AddModelError(string.Empty, $"API'den hata: {errorContent}");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, $"API Hatası (ham): {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[Gallery-Add] API bağlantı hatası: {ex.Message}");
                ModelState.AddModelError(string.Empty, "API sunucusuna ulaşılamadı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Gallery-Add] Genel hata: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Beklenmeyen bir hata oluştu.");
            }
            return View(dto);
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateGalleryImageDto? dto = await client.GetFromJsonAsync<UpdateGalleryImageDto>($"api/GalleryImages/{id}");
                if (dto == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Görsel bulunamadı.";
                    return RedirectToAction("Index");
                }
                if (dto.ImageByte != null && dto.ImageByte.Length > 0)
                    dto.ImageBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(dto.ImageByte)}";
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantı hatası.";
                Console.WriteLine($"[Gallery-Update-GET] {ex.Message}");
                return RedirectToAction("Index");
            }
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateGalleryImageDto dto)
        {
            try
            {
                var formFile = Request.Form.Files.GetFile("ImageFile");
                if (formFile is { Length: > 0 })
                {
                    string[] allowedTypes = { "image/jpeg", "image/png", "image/webp" };
                    long maxSize = 5 * 1024 * 1024;

                    if (!allowedTypes.Contains(formFile.ContentType))
                        ModelState.AddModelError(nameof(dto.ImageFile), "Geçersiz dosya türü. (jpg, png, webp)");
                    else if (formFile.Length > maxSize)
                        ModelState.AddModelError(nameof(dto.ImageFile), "Görsel en fazla 5 MB olabilir.");
                    else
                    {
                        await using MemoryStream ms = new MemoryStream();
                        await formFile.CopyToAsync(ms);
                        dto.ImageByte = ms.ToArray();
                        dto.ImageBase64 = $"data:{formFile.ContentType};base64,{Convert.ToBase64String(dto.ImageByte)}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(dto.ImageBase64))
                {
                    try
                    {
                        string base64Data = dto.ImageBase64.Split(',').Last();
                        dto.ImageByte = Convert.FromBase64String(base64Data);
                    }
                    catch (FormatException)
                    {
                        ModelState.AddModelError(nameof(dto.ImageFile), "Base64 görsel verisi çözümlenemedi.");
                    }
                }
                else
                    ModelState.AddModelError(nameof(dto.ImageFile), "Görsel yüklemek zorunludur.");
                if (!ModelState.IsValid)
                    return View(dto);
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/GalleryImages/{dto.ImageID}", dto);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Görsel başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                string content = await response.Content.ReadAsStringAsync();
                try
                {
                    ValidationProblemDetails problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (problemDetails?.Errors != null)
                    {
                        foreach (var error in problemDetails.Errors)
                        {
                            foreach (string msg in error.Value)
                                ModelState.AddModelError(error.Key, msg);
                        }
                    }
                    else
                        ModelState.AddModelError(string.Empty, $"API'den hata: {content}");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, $"API hatası (ham): {content}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[Gallery-Update] API bağlantı hatası: {ex.Message}");
                ModelState.AddModelError(string.Empty, "API sunucusuna ulaşılamadı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Gallery-Update] Genel hata: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Beklenmeyen bir hata oluştu.");
            }
            return View(dto);
        }
        [HttpPost("Sil/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"api/GalleryImages/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Görsel silindi.";
                }
                else
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = $"Sunucu hatası: {(int)response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantı hatası.";
                Console.WriteLine($"[Gallery-Delete] {ex.Message}");
            }
            return RedirectToAction("Index");
        }
        [HttpPost("DurumGuncelle/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, bool newStatus)
        {
            UpdateGalleryImageStatusDto dto = new UpdateGalleryImageStatusDto()
            {
                ImageID = id,
                ImageStatus = newStatus
            };
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"api/GalleryImages/{id}/status")
            {
                Content = JsonContent.Create(dto)
            };
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Resim durumu başarıyla güncellendi.";
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
                    Console.WriteLine($"[Resim-StatusUpdate] Sunucu yanıtı: {error}");
                }
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantı hatası oluştu.";
                Console.WriteLine($"[Resim-StatusUpdate] Exception: {ex.Message}");
            }
            return RedirectToAction("Index");
        }
    }
}