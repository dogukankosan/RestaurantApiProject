using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.CompanyInfoDtos;
using System.Net;
namespace RestaurantWebUI.Controllers
{
    [Route("AdminSirket")]
    public class AdminCompanyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminCompanyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/CompanyInfos");
                if (response.IsSuccessStatusCode)
                {
                    ResultCompanyInfoDto? result = await response.Content.ReadFromJsonAsync<ResultCompanyInfoDto>();
                    return View(result);
                }
                TempData["Type"] = "error";
                TempData["Message"] = "Şirket bilgileri alınamadı.";
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CompanyInfo-GET] Hata: {ex.Message}");
                TempData["Type"] = "error";
                TempData["Message"] = "API ile bağlantı kurulamadı.";
                return View();
            }
        }
        [HttpGet("Guncelle")]
        public async Task<IActionResult> Update()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/CompanyInfos");
                if (response.IsSuccessStatusCode)
                {
                    UpdateCompanyInfoDto? companyInfo = await response.Content.ReadFromJsonAsync<UpdateCompanyInfoDto>();
                    if (companyInfo is null)
                    {
                        TempData["Type"] = "error";
                        TempData["Message"] = "Şirket bilgileri bulunamadı.";
                        return RedirectToAction("Index");
                    }
                    return View(companyInfo); 
                }
                TempData["Type"] = "error";
                TempData["Message"] = "Şirket bilgileri alınamadı.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CompanyInfo-GET-Update] Hata: {ex.Message}");
                TempData["Type"] = "error";
                TempData["Message"] = "API bağlantısı sağlanamadı.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost("Guncelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCompanyInfoDto dto)
        {
            dto.CompanyInfoWebSiteLink ??= string.Empty;
            dto.CompanyInfoGithubLink ??= string.Empty;
            dto.CompanyInfoInstagramLink ??= string.Empty;
            dto.CompanyInfoLinkedinLink ??= string.Empty;
            if (!ModelState.IsValid)
                return View(dto);
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/CompanyInfos", dto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Şirket bilgileri başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (problem?.Errors != null)
                    {
                        foreach (var kv in problem.Errors)
                        {
                            foreach (string msg in kv.Value)
                                ModelState.AddModelError(kv.Key, msg);
                        }
                    }
                    else
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"Geçersiz veri gönderildi: {content}");
                    }
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Sunucu hatası: {(int)response.StatusCode} - {response.StatusCode}\n{content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CompanyInfo-POST-Update] API hatası: {ex.Message}");
                ModelState.AddModelError("", "Sunucuyla bağlantı kurulamadı. Lütfen daha sonra tekrar deneyin.");
            }
            return View(dto);
        }
    }
}