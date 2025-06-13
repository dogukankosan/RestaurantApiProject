using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.AboutDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class ReservationImageComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ReservationImageComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ResultAboutDto? aboutDto = null;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
                aboutDto = await client.GetFromJsonAsync<ResultAboutDto>("api/Abouts");
            }
            catch (HttpRequestException)
            {
                // TODO: loglama ekle
                aboutDto = null;    
            }
            byte[]? reservationImage = aboutDto?.AboutRezervationImage;
            return View("Default", reservationImage);
        }
    }
}