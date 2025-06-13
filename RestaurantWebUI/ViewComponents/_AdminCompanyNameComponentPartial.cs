using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.AboutDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class _AdminCompanyNameComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _AdminCompanyNameComponentPartial(IHttpClientFactory httpClientFactory)
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
                // todo: loglama yapılacak
                aboutDto = null;
            }
            (byte[]? logo ,string? companyName) abTuple= (aboutDto?.AboutCompanyLogo, aboutDto?.AboutCompanyName);
            return View("Default",abTuple);
        }
    }
}