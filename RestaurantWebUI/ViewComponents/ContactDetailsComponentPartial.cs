using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.CompanyInfoDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ContactDetailsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ContactDetailsComponentPartial(IHttpClientFactory httpClientFactory)
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
                // TODO: add logging here
                infoDto = null;
            }
            return View(infoDto);
        }
    }
}