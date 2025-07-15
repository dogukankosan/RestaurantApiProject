using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.EmailDtos;
using RestaurantWebUI.HelpersMethod;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminEmail")]
    public class AdminEmailController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminEmailController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateEmailDto? data = await client.GetFromJsonAsync<UpdateEmailDto>("api/Emails");
                if (data == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Mail ayarları bulunamadı.";
                    return View(new UpdateEmailDto());
                }
                data.EmailImageBase64 = ConvertToBase64(data.EmailImage);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mail ayarları yüklenirken bir hata oluştu.";
                return View(new UpdateEmailDto());
            }
        }

        [HttpGet("Guncelle")]
        public async Task<IActionResult> Update()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateEmailDto? email = await client.GetFromJsonAsync<UpdateEmailDto>("api/Emails");
                if (email == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Mail ayarları bulunamadı.";
                    return RedirectToAction("Index");
                }
                email.EmailPassword = null;
                email.EmailImageBase64 = ConvertToBase64(email.EmailImage);
                return View(email);
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mail ayarları yüklenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateEmailDto dto, IFormFile? EmailImageFile)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateEmailDto? existing = await client.GetFromJsonAsync<UpdateEmailDto>("api/Emails");
                if (existing == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Mevcut mail ayarları alınamadı.";
                    return RedirectToAction("Index");
                }
                if (string.IsNullOrWhiteSpace(dto.EmailPassword))
                    dto.EmailPassword = EncryptionHelper.Decrypt(existing.EmailPassword);
                dto.EmailCompanyName = dto.EmailCompanyName?.Trim() ?? string.Empty;
                dto.EmailAddress = dto.EmailAddress?.Trim() ?? string.Empty;
                dto.EmailPhone = string.IsNullOrWhiteSpace(dto.EmailPhone) || dto.EmailPhone.Trim().Length <= 5
                    ? string.Empty
                    : dto.EmailPhone.Trim();
                bool.TryParse(Request.Form["RemoveImage"], out bool removeImage);
                if (removeImage)
                    dto.EmailImage = Array.Empty<byte>();
                else if (EmailImageFile != null && EmailImageFile.Length > 0)
                {
                    using MemoryStream ms = new();
                    await EmailImageFile.CopyToAsync(ms);
                    dto.EmailImage = ms.ToArray();
                }
                else
                    dto.EmailImage = DecodeBase64(dto.EmailImageBase64) ?? existing.EmailImage;
                dto.EmailImageBase64 = ConvertToBase64(dto.EmailImage);
                using MultipartFormDataContent formData = new MultipartFormDataContent
                {
                    { new StringContent(dto.EmailID.ToString()), nameof(dto.EmailID) },
                    { new StringContent(dto.EmailBox ?? ""), nameof(dto.EmailBox) },
                    { new StringContent(dto.EmailPassword ?? ""), nameof(dto.EmailPassword) },
                    { new StringContent(dto.EmailServer ?? ""), nameof(dto.EmailServer) },
                    { new StringContent(dto.EmailSSl.ToString()), nameof(dto.EmailSSl) },
                    { new StringContent(dto.EmailPort.ToString()), nameof(dto.EmailPort) },
                    { new StringContent(dto.EmailCompanyName), nameof(dto.EmailCompanyName) },
                    { new StringContent(dto.EmailAddress), nameof(dto.EmailAddress) },
                    { new StringContent(dto.EmailPhone), nameof(dto.EmailPhone) },
                    { new StringContent(Convert.ToBase64String(dto.EmailImage ?? Array.Empty<byte>())), nameof(dto.EmailImage) }
                };
                HttpResponseMessage response = await client.PutAsync("api/Emails", formData);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Mail ayarları başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails? validation = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (validation?.Errors != null)
                    {
                        foreach (var error in validation.Errors)
                        {
                            string? key = error.Key == "EmailImage" ? "EmailImageFile" : error.Key;
                            foreach (string msg in error.Value)
                                ModelState.AddModelError(key, msg);
                        }
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Doğrulama hatası oluştu.");
                }
                if (!response.IsSuccessStatusCode)
                {
                    string? errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        ProblemDetails? problem = JsonSerializer.Deserialize<ProblemDetails>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (problem != null)
                        {
                            TempData["Type"] = "error";
                            TempData["Message"] = problem.Detail ?? "Mail gönderilirken bir hata oluştu.";
                        }
                        else
                            ModelState.AddModelError(string.Empty, $"API hatası ({(int)response.StatusCode}): {errorContent}");
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, $"API hatası ({(int)response.StatusCode}): {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Sunucuyla bağlantı hatası: {ex.Message}");
            }
            return View("Update", dto);
        }
        [HttpPost("Sifirla")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.PutAsync("api/Emails/Reset", null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Mail ayarları varsayılana sıfırlandı.";
                    return RedirectToAction("Index");
                }
                string content = await response.Content.ReadAsStringAsync();
                try
                {
                    ProblemDetails? problem = JsonSerializer.Deserialize<ProblemDetails>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    TempData["Type"] = "error";
                    TempData["Message"] = problem?.Detail ?? "Mail ayarları sıfırlanamadı.";
                }
                catch
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = $"Mail ayarları sıfırlanamadı: {content}";
                }
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = $"Sunucu hatası: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
        private byte[]? DecodeBase64(string? base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return null;
            try
            {
                string[] parts = base64.Split(',');
                string base64Data = parts.Length > 1 ? parts[1] : parts[0];
                return Convert.FromBase64String(base64Data);
            }
            catch
            {
                return null;
            }
        }
        private string? ConvertToBase64(byte[]? bytes)
        {
            return (bytes != null && bytes.Length > 0)
                ? $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}"
                : null;
        }
    }
}