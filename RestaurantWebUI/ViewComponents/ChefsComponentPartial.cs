using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ChefDtos;
using System.Net.Http.Json;

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
                List<ResultChefDto> response = await client.GetFromJsonAsync<List<ResultChefDto>>("api/Chefs");
                chefs = response?
                    .Where(c => c.ChefStatus) 
                    .ToList() ?? new List<ResultChefDto>();
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