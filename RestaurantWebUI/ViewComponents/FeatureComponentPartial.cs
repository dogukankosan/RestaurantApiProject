using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.FeatureDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class FeatureComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public FeatureComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ResultFeatureDto? featureDto;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                featureDto = await client.GetFromJsonAsync<ResultFeatureDto>("api/Features");
            }
            catch (HttpRequestException)
            {
                // TODO: add logging here
                featureDto = null;
            }
            return View(featureDto);
        }
    }
}