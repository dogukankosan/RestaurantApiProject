using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.CompanyInfoDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class AboutPhoneComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AboutPhoneComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ResultCompanyInfoDto? infoDto;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                infoDto = await client.GetFromJsonAsync<ResultCompanyInfoDto>("api/CompanyInfos");
            }
            catch (HttpRequestException)
            {
                // TODO: loglama ekle
                infoDto = null;
            }
            string? phone = infoDto?.CompanyInfoPhone;
            return View("Default",phone);
        }
    }
}