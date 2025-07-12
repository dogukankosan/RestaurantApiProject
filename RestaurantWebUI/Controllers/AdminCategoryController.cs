using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.CategoryDtos;
using System.Net;

namespace RestaurantWebUI.Controllers
{
    [Route("AdminKategori")]
    public class AdminCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("Liste")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                 List< ResultCategoryDto> categories = await client.GetFromJsonAsync<List<ResultCategoryDto>>("api/Categories");
                return View(categories ?? new List<ResultCategoryDto>());
            }
            catch (Exception ex)
            {
                // TODO: Logla (ex)
                TempData["Type"] = "error";
                TempData["Message"] = "Kategoriler yüklenirken bir hata oluştu.";
                return View(new List<ResultCategoryDto>());
            }
        }
        [HttpGet("Ekle")]
        public IActionResult Add()
        {
            return View(new CreateCategoryDto());
        }
        [HttpPost("Ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateCategoryDto createDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Categories", createDto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Kategori başarıyla eklendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (problem?.Errors != null)
                    {
                        foreach (var error in problem.Errors)
                            foreach (string msg in error.Value)
                                ModelState.AddModelError(error.Key, msg);
                        // TODO: Logla - validasyon hataları
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kategori eklenemedi.");
                    // TODO: Logla - bilinmeyen ekleme hatası
                }
            }
            catch (Exception ex)
            {
                // TODO: Logla (ex)
                ModelState.AddModelError("", "Sunucuyla bağlantı kurulamadı.");
            }
            return View(createDto);
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateCategoryDto category = await client.GetFromJsonAsync<UpdateCategoryDto>($"api/Categories/{id}");
                if (category == null)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Kategori bulunamadı.";
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            catch (Exception ex)
            {
                // TODO: Logla (ex)
                TempData["Type"] = "error";
                TempData["Message"] = "Kategori bilgisi alınırken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost("Guncelle/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto updateDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Categories/{id}", updateDto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Kategori başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ValidationProblemDetails problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    if (problem?.Errors != null)
                    {
                        foreach (var error in problem.Errors)
                            foreach (string msg in error.Value)
                                ModelState.AddModelError(error.Key, msg);
                        // TODO: Logla - validasyon hataları
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kategori güncellenemedi.");
                    // TODO: Logla - bilinmeyen güncelleme hatası
                }
            }
            catch (Exception ex)
            {
                // TODO: Logla (ex)
                ModelState.AddModelError("", "Sunucuyla bağlantı kurulamadı.");
            }
            return View(updateDto);
        }
        [HttpPost("DurumGuncelle/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string newStatus)
        {
            if (!bool.TryParse(newStatus, out bool parsedStatus))
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Geçersiz durum değeri gönderildi.";
                return RedirectToAction("Index");
            }
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            UpdateCategoryStatusDto statusDto = new UpdateCategoryStatusDto
            {
                CategoryID = id,
                CategoryStatus = parsedStatus
            };
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"api/Categories/{id}/Status")
            {
                Content = JsonContent.Create(statusDto)
            };
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Type"] = "success";
                    TempData["Message"] = "Kategori durumu başarıyla güncellendi.";
                }
                else
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Kategori durumu güncellenemedi.";
                    // TODO: Logla - durum güncelleme hatası
                }
            }
            catch (Exception ex)
            {
                // TODO: Logla (ex)
                TempData["Type"] = "error";
                TempData["Message"] = "Durum güncelleme sırasında bir hata oluştu.";
            }
            return RedirectToAction("Index");
        }
    }
}