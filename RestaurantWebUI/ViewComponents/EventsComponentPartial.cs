using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.EventDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class EventsComponentPartial : ViewComponent
    {
        private const string CacheKey = "EventsDtoList";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public EventsComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<List<ResultEventDto>>(CacheKey, out var events))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    events = await client.GetFromJsonAsync<List<ResultEventDto>>("api/Events");
                    if (events != null)
                        _cache.Set(CacheKey, events, CacheDuration);
                }
                catch (HttpRequestException)
                {
                    // TODO: add logging here
                    events = new List<ResultEventDto>();
                }
            }
            return View(events);
        }
    }
}