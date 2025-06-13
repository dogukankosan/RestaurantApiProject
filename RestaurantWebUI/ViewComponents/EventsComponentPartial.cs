using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.EventDtos;

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
                events = await client.GetFromJsonAsync<List<ResultEventDto>>("api/Events")
                         ?? new List<ResultEventDto>();
            }
            catch (HttpRequestException)
            {
                // TODO: add logging here
                events = new List<ResultEventDto>();
            }
            return View(events);
        }
    }
}