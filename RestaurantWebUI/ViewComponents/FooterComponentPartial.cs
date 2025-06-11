using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.CompanyInfoDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class FooterComponentPartial : ViewComponent
    {
        private const string CacheKey = "CompanyssInfoDto";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public FooterComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<ResultCompanyInfoDto>(CacheKey, out var companyInfo))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    companyInfo = await client.GetFromJsonAsync<ResultCompanyInfoDto>("api/CompanyInfos");
                    if (companyInfo != null)
                        _cache.Set(CacheKey, companyInfo, CacheDuration);
                }
                catch (HttpRequestException)
                {
                    // TODO: add logging here
                    companyInfo = null;
                }
            }
            return View(companyInfo);
        }
    }
}