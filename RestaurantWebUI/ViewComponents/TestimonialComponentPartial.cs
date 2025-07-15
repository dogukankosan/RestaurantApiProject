using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.TestimonialDtos;
using System.Net.Http.Json;

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
                List<ResultTestimonialDto> response = await client.GetFromJsonAsync<List<ResultTestimonialDto>>("api/Testimonials");
                testimonials = response?
                    .Where(t => t.TestimonialStatus) 
                    .ToList() ?? new List<ResultTestimonialDto>();
            }
            catch (HttpRequestException)
            {
                testimonials = new List<ResultTestimonialDto>();
            }
            return View(testimonials);
        }
    }
}