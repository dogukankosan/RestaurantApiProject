using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.CompanyInfoDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class FooterComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public FooterComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ResultCompanyInfoDto? companyInfo;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                companyInfo = await client.GetFromJsonAsync<ResultCompanyInfoDto>("api/CompanyInfos");
            }
            catch (HttpRequestException)
            {
                // TODO: add logging here
                companyInfo = null;
            }
            return View(companyInfo);
        }
    }
}