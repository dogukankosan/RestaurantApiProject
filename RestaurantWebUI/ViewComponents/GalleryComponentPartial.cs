using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.GalleryImageDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class GalleryComponentPartial : ViewComponent
    {
        private const string CacheKey = "GalleryImagesDtoList";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public GalleryComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<List<ResultGalleryImageDto>>(CacheKey, out var galleryImages))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    galleryImages = await client.GetFromJsonAsync<List<ResultGalleryImageDto>>("api/GalleryImages");
                    if (galleryImages != null) 
                        _cache.Set(CacheKey, galleryImages, CacheDuration);
                    else
                        galleryImages = new List<ResultGalleryImageDto>();
                }
                catch (HttpRequestException)
                {
                    // TODO: add logging here
                    galleryImages = new List<ResultGalleryImageDto>();
                }
            }
            return View(galleryImages);
        }
    }
}