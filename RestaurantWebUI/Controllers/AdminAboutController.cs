using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.AboutDtos;
using System.Net;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminHakkimizda")]
    public class AdminAboutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminAboutController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                ResultAboutDto? data = await client.GetFromJsonAsync<ResultAboutDto>("api/Abouts");
                if (data == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Hakkında bilgisi bulunamadı.";
                    return View(new ResultAboutDto());
                }
                return View(data);
            }
            catch (Exception)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Hakkında yüklenirken bir hata oluştu.";
                // TODO: Loglanacak - Hakkında bilgisi alınamadı
                return View(new ResultAboutDto());
            }
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateAboutDto? about = await client.GetFromJsonAsync<UpdateAboutDto>("api/Abouts");
                if (about == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Hakkımızda bilgisi bulunamadı.";
                    return RedirectToAction("Index");
                }
                return View(about);
            }
            catch (Exception)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Hakkımızda bilgisi alınırken bir hata oluştu.";
                // TODO: Loglanacak - Detay çekilemedi
                return RedirectToAction("Index");
            }
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateAboutDto updateDto,
            IFormFile? AboutCompanyLogoFile,
            IFormFile? AboutImage1File,
            IFormFile? AboutImage2File,
            IFormFile? AboutReportImageFile,
            IFormFile? AboutRezervationImageFile)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateAboutDto? existing = await client.GetFromJsonAsync<UpdateAboutDto>("api/Abouts");
                if (existing == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Mevcut hakkımızda verisi alınamadı.";
                    return RedirectToAction("Index");
                }
                updateDto.AboutCompanyLogo = await GetOrKeep(existing.AboutCompanyLogo, AboutCompanyLogoFile, updateDto.AboutCompanyLogoBase64);
                updateDto.AboutImage1 = await GetOrKeep(existing.AboutImage1, AboutImage1File, updateDto.AboutImage1Base64);
                updateDto.AboutImage2 = await GetOrKeep(existing.AboutImage2, AboutImage2File, updateDto.AboutImage2Base64);
                updateDto.AboutReportImage = await GetOrKeep(existing.AboutReportImage, AboutReportImageFile, updateDto.AboutReportImageBase64);
                updateDto.AboutRezervationImage = await GetOrKeep(existing.AboutRezervationImage, AboutRezervationImageFile, updateDto.AboutRezervationImageBase64);

                updateDto.AboutCompanyLogoBase64 = ConvertToBase64(updateDto.AboutCompanyLogo);
                updateDto.AboutImage1Base64 = ConvertToBase64(updateDto.AboutImage1);
                updateDto.AboutImage2Base64 = ConvertToBase64(updateDto.AboutImage2);
                updateDto.AboutReportImageBase64 = ConvertToBase64(updateDto.AboutReportImage);
                updateDto.AboutRezervationImageBase64 = ConvertToBase64(updateDto.AboutRezervationImage);
                HttpResponseMessage response = await client.PutAsJsonAsync("api/Abouts", updateDto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Hakkımızda başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (problem?.Errors != null)
                    {
                        foreach (KeyValuePair<string, string[]> error in problem.Errors)
                        {
                            string key = error.Key switch
                            {
                                "AboutCompanyLogo" => "AboutCompanyLogoFile",
                                "AboutImage1" => "AboutImage1File",
                                "AboutImage2" => "AboutImage2File",
                                "AboutReportImage" => "AboutReportImageFile",
                                "AboutRezervationImage" => "AboutRezervationImageFile",
                                _ => error.Key
                            };
                            foreach (string msg in error.Value)
                            {
                                ModelState.AddModelError(key, msg);
                            }
                        }
                    }
                }
                else
                {
                    string apiError = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API hatası ({(int)response.StatusCode}): {apiError}");
                    // TODO: Loglanacak - API bilinmeyen hata
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Sunucuyla bağlantı sırasında bir hata oluştu.");
                // TODO: Loglanacak - API çağrısı başarısız
            }

            TempData["Type"] = "error";
            TempData["Message"] = "Güncelleme sırasında hata oluştu.";
            return View(updateDto);
        }
        private byte[]? DecodeBase64(string? base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return null;
            string[] parts = base64.Split(',');
            string data = parts.Length > 1 ? parts[1] : parts[0];
            return Convert.FromBase64String(data);
        }
        private async Task<byte[]?> GetOrKeep(byte[]? current, IFormFile? file, string? base64)
        {
            if (file != null && file.Length > 0)
            {
                using MemoryStream ms = new MemoryStream();
                await file.CopyToAsync(ms);
                return ms.ToArray();
            }
            if (current != null && current.Length > 0)
                return current;
            return DecodeBase64(base64);
        }
        private string? ConvertToBase64(byte[]? bytes)
        {
            return (bytes != null && bytes.Length > 0)
                ? $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}"
                : null;
        }
    }
}