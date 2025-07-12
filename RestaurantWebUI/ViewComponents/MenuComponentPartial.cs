using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.CategoryDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class MenuComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MenuComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ResultCategoryDto> categories = new();
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                List<ResultCategoryDto> result = await client.GetFromJsonAsync<List<ResultCategoryDto>>("api/Categories");
                categories = result?.Where(c => c.CategoryStatus).ToList() ?? new();
            }
            catch (HttpRequestException)
            {
                // TODO: Loglama yapılacak
                categories = new List<ResultCategoryDto>();
            }
            return View(categories);
        }
    }
}