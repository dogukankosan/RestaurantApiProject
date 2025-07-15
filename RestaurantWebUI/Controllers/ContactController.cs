using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.MessageDtos;
using RestaurantWebUI.HelpersMethod;
using System.Net;
using System.Text.Json;

[AutoValidateAntiforgeryToken]
public class ContactController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public ContactController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromForm] CreateMessageDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
            return Json(new { success = false, errors });
        }
        string? ip = WebLogHelper.GetUserIp(HttpContext);
        string? userAgent = WebLogHelper.GetUserAgent(HttpContext);
        string? geoInfo = await WebLogHelper.GetGeoInfoAsync(ip);
        dto.WebLogUserIPAdress = ip;
        dto.WebLogUserInfo = userAgent;
        dto.WebLogUserGeo = geoInfo;
        dto.MessageSendDate = DateTime.Now;
        dto.MessageIsRead = false;
        HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
        HttpResponseMessage response = await client.PostAsJsonAsync("api/Messages", dto);
        if (response.IsSuccessStatusCode)
        {
            return Json(new
            {
                success = true,
                message = "👌 Mesajınız başarıyla iletildi.En kısa süre içinde sizinle iletişime geçilecektir."
            });
        }
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            string? errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                ValidationProblemDetails? problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(errorContent);
                var apiErrors = problemDetails?.Errors?.SelectMany(e => e.Value).ToList()
                                 ?? new List<string> { "Geçersiz istek" };
                return Json(new { success = false, errors = apiErrors });
            }
            catch
            {
                return Json(new { success = false, errors = new List<string> { errorContent } });
            }
        }
        return Json(new
        {
            success = false,
            errors = new List<string> { "Sunucu hatası oluştu. Lütfen tekrar deneyin." }
        });
    }
}