using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReportDtos;
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
            ResultUserReportDto? reportDto;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                reportDto = await client.GetFromJsonAsync<ResultUserReportDto>("api/Reports/summary");
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