using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ProductDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ProductsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            List<ResultProductDto> products = new();
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                List<ResultProductDto> response = await client.GetFromJsonAsync<List<ResultProductDto>>("api/Products");
                if (response != null)
                {
                    products = response
                        .Where(p => p.ProductStatus && p.CategoryID == categoryId)
                        .ToList();
                }
            }
            catch (HttpRequestException)
            {
                // TODO: Loglama yapılacak
                products = new();
            }
            return View(products);
        }
    }
}