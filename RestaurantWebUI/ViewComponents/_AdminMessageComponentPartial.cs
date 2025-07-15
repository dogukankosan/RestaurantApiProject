using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.MessageDtos;

namespace RestaurantWebUI.ViewComponents
{
    public class _AdminMessageComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _AdminMessageComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ResultMessageDto>? messageDto = null;
            HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
            try
            {
               messageDto = await client.GetFromJsonAsync<List<ResultMessageDto>>("api/Messages");
            }
            catch (HttpRequestException)
            {
                // todo: loglama yapılacak
                messageDto = null;
            }
            List<ResultMessageDto> unreadMessages = messageDto?
                                     .Where(m => m.MessageIsRead == false)
                                     .ToList()
                                 ?? new List<ResultMessageDto>();
            return View(unreadMessages);
        }
    }
}