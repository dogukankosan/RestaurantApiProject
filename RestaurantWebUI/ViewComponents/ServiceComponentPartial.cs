using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ServiceDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ServiceComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ServiceComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ResultServiceDto> services;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                services = await client.GetFromJsonAsync<List<ResultServiceDto>>("api/Services")
                           ?? new List<ResultServiceDto>();
            }
            catch (HttpRequestException)
            {
                services = new List<ResultServiceDto>();
            }
            return View(services);
        }
    }
}