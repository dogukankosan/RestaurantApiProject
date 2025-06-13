using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Models;
namespace RestaurantWebUI.ViewComponents
{
    public class ReportComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ReportComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Report? reportDto;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                reportDto = await client.GetFromJsonAsync<Report>("api/Reports");
            }
            catch (HttpRequestException)
            {
                // TODO: add logging here
                reportDto = null;
            }
            return View(reportDto);
        }
    }
}