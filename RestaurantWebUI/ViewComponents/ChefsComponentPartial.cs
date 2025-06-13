using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ChefDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ChefsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ChefsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ResultChefDto> chefs;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                chefs = await client.GetFromJsonAsync<List<ResultChefDto>>("api/Chefs")
                        ?? new List<ResultChefDto>();
            }
            catch (HttpRequestException)
            {
                // TODO: loglama ekle
                chefs = new List<ResultChefDto>();
            }
            return View(chefs);
        }
    }
}