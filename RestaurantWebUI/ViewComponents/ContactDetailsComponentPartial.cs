using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.CompanyInfoDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ContactDetailsComponentPartial : ViewComponent
    {
        private const string CacheKey = "CompanyInfosDto";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public ContactDetailsComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<ResultCompanyInfoDto>(CacheKey, out var infoDto))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    infoDto = await client.GetFromJsonAsync<ResultCompanyInfoDto>("api/CompanyInfos");
                    if (infoDto != null)
                        _cache.Set(CacheKey, infoDto, CacheDuration);
                }
                catch (HttpRequestException)
                {
                    // TODO: add logging here
                    infoDto = null;
                }
            }
            return View(infoDto);
        }
    }
}