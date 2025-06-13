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
            List<ResultCategoryDto> categories = new();
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                List<ResultCategoryDto>? data = await client.GetFromJsonAsync<List<ResultCategoryDto>>("api/Categories");
                categories = data ?? new List<ResultCategoryDto>();
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Kategoriler yüklenirken bir problem oluştu.";
                // TODO: Loglanacak - API'den kategori listesi çekilirken hata
            }
            return View(categories);
        }
        [HttpGet("Ekle")]
        public IActionResult Add()
            => View(new CreateCategoryDto());
        [HttpPost("Ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateCategoryDto createDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync("api/Categories", createDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sunucuyla iletişimde hata oluştu.");
                // TODO: Loglanacak - API'ye bağlanılamadı
                return View(createDto);
            }
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
                    foreach (var kv in problem.Errors)
                    {
                        foreach (var msg in kv.Value)
                            ModelState.AddModelError(kv.Key, msg);
                        // TODO: Loglanacak - API validasyon hatası
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Kategori ekleme başarısız oldu.");
                // TODO: Loglanacak - API bilinmeyen hata
            }
            return View(createDto);
        }
        [HttpGet("Guncelle/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                UpdateCategoryDto? category = await client.GetFromJsonAsync<UpdateCategoryDto>($"api/Categories/{id}");
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
                TempData["Type"] = "error";
                TempData["Message"] = "Kategori bilgisi alınırken bir hata oluştu.";
                // TODO: Loglanacak - Kategori detayları çekilemedi
                return RedirectToAction("Index");
            }
        }
        [HttpPost("Guncelle/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto updateDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response;
            try
            {
                response = await client.PutAsJsonAsync($"api/Categories/{id}", updateDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sunucuyla iletişimde hata oluştu.");
                // TODO: Loglanacak - API'ye güncelleme isteği atılamadı
                return View(updateDto);
            }
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ValidationProblemDetails ? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                if (problem?.Errors != null)
                {
                    foreach (var kv in problem.Errors)
                    {
                        foreach (var msg in kv.Value)
                            ModelState.AddModelError(kv.Key, msg);
                        // TODO: Loglanacak - Güncelleme validasyon hatası
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Kategori güncelleme başarısız oldu.");
                // TODO: Loglanacak - API bilinmeyen güncelleme hatası
            }
            return View(updateDto);
        }
    }
}