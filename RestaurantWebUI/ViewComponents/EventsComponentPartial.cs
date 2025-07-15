using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.EventDtos;
using System.Net.Http.Json;

namespace RestaurantWebUI.ViewComponents
{
    public class EventsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ResultEventDto> events;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                List<ResultEventDto>? response = await client.GetFromJsonAsync<List<ResultEventDto>>("api/Events");
                events = response?
                    .Where(e => e.EventStatus)
                    .ToList() ?? new List<ResultEventDto>();
            }
            catch (HttpRequestException)
            {
                events = new List<ResultEventDto>();
            }
            return View(events);
        }
    }
}