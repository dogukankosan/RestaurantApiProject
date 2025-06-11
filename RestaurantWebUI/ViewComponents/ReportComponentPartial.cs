using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Models;

namespace RestaurantWebUI.ViewComponents
{
    public class ReportComponentPartial : ViewComponent
    {
        private const string CacheKey = "ReportDto";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public ReportComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<Report>(CacheKey, out var reportDto))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    reportDto = await client.GetFromJsonAsync<Report>("api/Reports");
                    if (reportDto != null)
                        _cache.Set(CacheKey, reportDto, CacheDuration);
                }
                catch (HttpRequestException)
                {
                    // TODO: add logging here
                    reportDto = null;
                }
            }
            return View(reportDto);
        }
    }
}