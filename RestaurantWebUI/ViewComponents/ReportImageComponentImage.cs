using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.AboutDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ReportImageComponentImage : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ReportImageComponentImage(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ResultAboutDto? aboutDto;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                aboutDto = await client.GetFromJsonAsync<ResultAboutDto>("api/Abouts");
            }
            catch (HttpRequestException)
            {
                // TODO: loglama ekle
                aboutDto = null;
            }
            byte[]? reportImage = aboutDto?.AboutReportImage;
            return View("Default",reportImage);
        }
    }
}