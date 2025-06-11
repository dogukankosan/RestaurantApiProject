using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.ChefDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ChefsComponentPartial : ViewComponent
    {
        private const string CacheKey = "ChefsDtoList";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public ChefsComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<List<ResultChefDto>>(CacheKey, out var chefs))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    chefs = await client.GetFromJsonAsync<List<ResultChefDto>>("api/Chefs");
                    if (chefs != null)
                        _cache.Set(CacheKey, chefs, CacheDuration);
                }
                catch (HttpRequestException)
                {
                    // TODO: loglama ekle
                    chefs = new List<ResultChefDto>();
                }
            }
            return View(chefs);
        }
    }
}