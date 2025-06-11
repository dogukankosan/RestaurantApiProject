using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.FeatureDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class FeatureComponentPartial : ViewComponent
    {
        private const string CacheKey = "FeatureDto";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public FeatureComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<ResultFeatureDto>(CacheKey, out var featureDto))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    featureDto = await client.GetFromJsonAsync<ResultFeatureDto>("api/Features");
                    if (featureDto != null)
                        _cache.Set(CacheKey, featureDto, CacheDuration);
                }
                catch (HttpRequestException)
                {
                    // TODO: add logging here
                    featureDto = null;
                }
            }
            return View(featureDto);
        }
    }
}