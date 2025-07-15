using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReportDtos;
using System.Net.Http;

namespace RestaurantWebUI.ViewComponents
{
    public class _AdminMonthReservationChartComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _AdminMonthReservationChartComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            List<ResultAdminMonthlyReservationStatsDto>? data;
            try
            {
                data = await client.GetFromJsonAsync<List<ResultAdminMonthlyReservationStatsDto>>("api/reports/monthly-reservations");
            }
            catch
            {
                data = new List<ResultAdminMonthlyReservationStatsDto>();
            }
            return View(data);
        }
    }
}