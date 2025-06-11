using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantWebUI.Dtos.TestimonialDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class TestimonialComponentPartial : ViewComponent
    {
        private const string CacheKey = "TestimonialsDtoList";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public TestimonialComponentPartial(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_cache.TryGetValue<List<ResultTestimonialDto>>(CacheKey, out var testimonials))
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
                try
                {
                    testimonials = await client.GetFromJsonAsync<List<ResultTestimonialDto>>("api/Testimonials");
                    if (testimonials != null)
                        _cache.Set(CacheKey, testimonials, CacheDuration);
                    else
                        testimonials = new List<ResultTestimonialDto>();
                }
                catch (HttpRequestException)
                {
                    // TODO: add logging here
                    testimonials = new List<ResultTestimonialDto>();
                }
            }
            return View(testimonials);
        }
    }
}