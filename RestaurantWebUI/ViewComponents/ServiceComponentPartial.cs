using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.ServiceDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ServiceComponentPartial : ViewComponent
    {
        private const string CacheKey = "ServicesDtoList";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public ServiceComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<List<ResultServiceDto>>(CacheKey, out var services))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    services = await client.GetFromJsonAsync<List<ResultServiceDto>>("api/Services");
                    if (services != null)
                        _cache.Set(CacheKey, services, CacheDuration);
                    else
                        services = new List<ResultServiceDto>();
                }
                catch (HttpRequestException)
                {
                    // TODO: add logging here
                    services = new List<ResultServiceDto>();
                }
            }
            return View(services);
        }
    }
}