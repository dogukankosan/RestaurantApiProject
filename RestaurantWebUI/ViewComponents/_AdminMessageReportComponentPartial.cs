using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReportDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class _AdminMessageReportComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _AdminMessageReportComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            List<ResultAdminMonthlyMessageStatsDto>? data;
            try
            {
                data = await client.GetFromJsonAsync<List<ResultAdminMonthlyMessageStatsDto>>("api/reports/monthly-message-stats");
            }
            catch
            {
                data = new List<ResultAdminMonthlyMessageStatsDto>();
            }
            return View(data);
        }
    }
}