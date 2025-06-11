using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.AboutDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ReportImageComponentImage:ViewComponent
    {
        private const string CacheKey = "reportImage";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public ReportImageComponentImage(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<ResultAboutDto>(CacheKey, out var aboutDto))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    aboutDto = await client.GetFromJsonAsync<ResultAboutDto>("api/Abouts");
                    if (aboutDto != null)
                        _cache.Set(CacheKey, aboutDto, CacheDuration);
                }
                catch (HttpRequestException)
                {
                    //todo loglamalar yapılacak
                    aboutDto = null;
                }
            }
            return View(aboutDto);
        }
    }
}