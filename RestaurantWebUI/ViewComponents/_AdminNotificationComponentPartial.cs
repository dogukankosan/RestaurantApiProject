using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReservationDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class _AdminNotificationComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _AdminNotificationComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ResultReservationDto>? rezDtos = null;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");

            try
            {
                rezDtos = await client.GetFromJsonAsync<List<ResultReservationDto>>("api/Reservations");
            }
            catch (HttpRequestException)
            {
                // todo: loglama yapılacak
                rezDtos = null;
            }
            List<ResultReservationDto> unreadMessages = rezDtos?
                                                            .Where(m => m.ReservationIsRead == false && m.ReservationStatus==false)
                                                            .ToList()
                                                        ?? new List<ResultReservationDto>();
            return View(unreadMessages);
        }
    }
}