using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReportDtos;
using System.Net.Http;

namespace RestaurantWebUI.ViewComponents
{
    public class _AdminMonthReservationChartStatusComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _AdminMonthReservationChartStatusComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            List<ResultAdminMonthlyReservationStatusDto>? data;
            try
            {
                data = await client.GetFromJsonAsync<List<ResultAdminMonthlyReservationStatusDto>>("api/reports/monthly-reservation-status");
            }
            catch
            {
                data = new List<ResultAdminMonthlyReservationStatusDto>();
            }
            return View(data);
        }
    }
}