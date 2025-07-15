using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.GalleryImageDtos;
using System.Net.Http.Json;

namespace RestaurantWebUI.ViewComponents
{
    public class GalleryComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GalleryComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ResultGalleryImageDto> galleryImages;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                List<ResultGalleryImageDto>? response = await client.GetFromJsonAsync<List<ResultGalleryImageDto>>("api/GalleryImages");
                galleryImages = response?
                    .Where(g => g.ImageStatus) 
                    .ToList() ?? new List<ResultGalleryImageDto>();
            }
            catch (HttpRequestException)
            {
                galleryImages = new List<ResultGalleryImageDto>();
            }
            return View(galleryImages);
        }
    }
}