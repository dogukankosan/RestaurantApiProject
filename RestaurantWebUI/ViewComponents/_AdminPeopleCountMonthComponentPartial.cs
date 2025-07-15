using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReportDtos;
using System.Net.Http;

namespace RestaurantWebUI.ViewComponents
{
    public class _AdminPeopleCountMonthComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _AdminPeopleCountMonthComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            List<ResultAdminMonthlyPeopleStatsDto>? data;
            try
            {
                data = await client.GetFromJsonAsync<List<ResultAdminMonthlyPeopleStatsDto>>("api/reports/monthly-arrived-people");
            }
            catch
            {
                data = new List<ResultAdminMonthlyPeopleStatsDto>();
            }
            return View(data);
        }
    }
}