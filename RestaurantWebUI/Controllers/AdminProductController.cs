using Microsoft.AspNetCore.Mvc;
using System.Net;
using RestaurantWebUI.Dtos.CategoryDtos;
using RestaurantWebUI.Dtos.ProductDtos;

[Route("AdminUrun")]
public class AdminProductController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public AdminProductController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    [HttpGet("Liste")]
    public async Task<IActionResult> Index()
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            List<ResultProductDto> data = await client.GetFromJsonAsync<List<ResultProductDto>>("api/Products");
            return View(data ?? new List<ResultProductDto>());
        }
        catch (Exception ex)
        {
            TempData["Type"] = "error";
            TempData["Message"] = "Ürünler yüklenirken bir hata oluştu.";
            // TODO: Loglama yapılabilir (ex)
            return View(new List<ResultProductDto>());
        }
    }

    [HttpGet("Ekle")]
    public async Task<IActionResult> Add()
    {
        CreateProductDto model = new CreateProductDto
        {
            CategoryList = await GetActiveCategoriesAsync()
        };
        return View(model);
    }

    [HttpPost("Ekle")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CreateProductDto createDto)
    {
        if (createDto.ProductImageFile != null && createDto.ProductImageFile.Length > 0)
        {
            using MemoryStream ms = new MemoryStream();
            await createDto.ProductImageFile.CopyToAsync(ms);
            createDto.ProductImageData = ms.ToArray();
            createDto.ProductImageBase64 = $"data:{createDto.ProductImageFile.ContentType};base64,{Convert.ToBase64String(createDto.ProductImageData)}";
        }
        else if (!string.IsNullOrWhiteSpace(createDto.ProductImageBase64))
        {
            try
            {
                string base64Data = createDto.ProductImageBase64.Split(',').Last();
                createDto.ProductImageData = Convert.FromBase64String(base64Data);
            }
            catch
            {
                ModelState.AddModelError("ProductImageFile", "Önceki yüklenen görsel okunamadı, lütfen yeniden yükleyin.");
            }
        }
        else
            ModelState.AddModelError("ProductImageFile", "Ürün görseli zorunludur.");
        createDto.ProductDescription ??= string.Empty;
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Products", createDto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Ürün başarıyla eklendi.";
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
                ModelState.AddModelError(string.Empty, "Ürün ekleme başarısız oldu.");
        }
        catch (Exception ex)
        {
            // TODO: Loglama yapılabilir (ex)
            ModelState.AddModelError(string.Empty, "Sunucuyla iletişimde hata oluştu.");
        }
        createDto.CategoryList = await GetActiveCategoriesAsync();
        return View(createDto);
    }
    [HttpGet("Guncelle/{id}")]
    public async Task<IActionResult> Update(int id)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            UpdateProductDto product = await client.GetFromJsonAsync<UpdateProductDto>($"api/Products/{id}");
            if (product is null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Ürün bulunamadı.";
                return RedirectToAction("Index");
            }
            product.CategoryList = await GetActiveCategoriesAsync();
            return View(product);
        }
        catch (Exception ex)
        {
            // TODO: Loglama yapılabilir (ex)
            TempData["Type"] = "error";
            TempData["Message"] = "Ürün bilgisi alınırken bir hata oluştu.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost("Guncelle")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateProductDto updateDto)
    {
        HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
        try
        {
            if (updateDto.ProductImageFile != null && updateDto.ProductImageFile.Length > 0)
            {
                MemoryStream  ms = new MemoryStream();
                await updateDto.ProductImageFile.CopyToAsync(ms);
                updateDto.ProductImageData = ms.ToArray();
            }
            else if (!string.IsNullOrWhiteSpace(updateDto.ProductImageBase64))
            {
                string? base64Data = updateDto.ProductImageBase64.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64Data))
                    updateDto.ProductImageData = Convert.FromBase64String(base64Data);
            }
            else
            {
                UpdateProductDto? existing = await client.GetFromJsonAsync<UpdateProductDto>($"api/Products/{updateDto.ProductID}");
                if (existing != null && existing.ProductImageData != null)
                    updateDto.ProductImageData = existing.ProductImageData;
            }
            updateDto.ProductDescription ??= string.Empty;
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Products/{updateDto.ProductID}", updateDto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Ürün başarıyla güncellendi.";
                return RedirectToAction("Liste","AdminUrun");
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
                ModelState.AddModelError(string.Empty, "Ürün güncelleme başarısız oldu.");
        }
        catch (Exception ex)
        {
            // TODO: ex loglanabilir
            ModelState.AddModelError(string.Empty, "Sunucuyla iletişimde hata oluştu.");
        }
        updateDto.CategoryList = await GetActiveCategoriesAsync();
        return View(updateDto);
    }

    [HttpPost("DurumGuncelle/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string newStatus)
    {
        HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
        bool parsed = bool.TryParse(newStatus, out bool status);
        UpdateProductStatusDto statusDto = new UpdateProductStatusDto
        {
            ProductID = id,
            ProductStatus = parsed && status
        };
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"api/Products/{id}/Status")
            {
                Content = JsonContent.Create(statusDto)
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Ürün durumu başarıyla güncellendi.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Ürün durumu güncellenemedi.";
            }
        }
        catch (Exception ex)
        {
            TempData["Type"] = "error";
            TempData["Message"] = "Durum güncelleme sırasında bir hata oluştu.";
            // TODO: Logla (ex)
        }
        return RedirectToAction("Index");
    }
    [HttpGet("Sil/{id}")]
    public async Task<IActionResult> Sil(int id)
    {
        HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
        try
        {
            HttpResponseMessage response = await client.DeleteAsync($"api/Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Type"] = "success";
                TempData["Message"] = "Ürün başarıyla silindi.";
            }
            else
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Ürün silinirken bir hata oluştu.";
            }
        }
        catch (Exception ex)
        {
            TempData["Type"] = "error";
            TempData["Message"] = "Sunucuyla iletişimde hata oluştu.";
            // TODO: Logla (ex)
        }
        return RedirectToAction("Liste","AdminUrun");
    }
    private async Task<List<ResultCategoryDto>> GetActiveCategoriesAsync()
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            List<ResultCategoryDto> data = await client.GetFromJsonAsync<List<ResultCategoryDto>>("api/Categories");
            return data?.Where(x => x.CategoryStatus).ToList() ?? new();
        }
        catch (Exception ex)
        {
            TempData["Type"] = "error";
            TempData["Message"] = "Kategoriler yüklenemedi.";
            // TODO: Loglama yapılabilir (ex)
            return new();
        }
    }
}