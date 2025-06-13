using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.TestimonialDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class TestimonialComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TestimonialComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ResultTestimonialDto> testimonials;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                testimonials = await client.GetFromJsonAsync<List<ResultTestimonialDto>>("api/Testimonials")
                               ?? new List<ResultTestimonialDto>();
            }
            catch (HttpRequestException)
            {
                // TODO: add logging here
                testimonials = new List<ResultTestimonialDto>();
            }
            return View(testimonials);
        }
    }
}