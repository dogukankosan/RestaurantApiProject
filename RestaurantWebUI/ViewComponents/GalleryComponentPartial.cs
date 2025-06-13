using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.GalleryImageDtos;

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
                galleryImages = await client.GetFromJsonAsync<List<ResultGalleryImageDto>>("api/GalleryImages")
                                ?? new List<ResultGalleryImageDto>();
            }
            catch (HttpRequestException)
            {
                // TODO: add logging here
                galleryImages = new List<ResultGalleryImageDto>();
            }
            return View(galleryImages);
        }
    }
}