using Microsoft.AspNetCore.Mvc;
using RestaurantWebUI.Dtos.ReservationDtos;
using RestaurantWebUI.HelpersMethod;
using System.Net;
using System.Text.Json;

[AutoValidateAntiforgeryToken]
public class ReservationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public ReservationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromForm] CreateReservationDto dto, [FromForm] string ReservationTime)
    {
        if (!ModelState.IsValid)
        {
            List<string>? errors = ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
            return Json(new { success = false, errors });
        }
        string? ip = WebLogHelper.GetUserIp(HttpContext);
        string? agent = WebLogHelper.GetUserAgent(HttpContext);
        string? geo = await WebLogHelper.GetGeoInfoAsync(ip);
        dto.WebLogUserIPAdress = ip;
        dto.WebLogUserInfo = agent;
        dto.WebLogUserGeo = geo;
        dto.ReservationMessage= dto.ReservationMessage==null? "":dto.ReservationMessage;
        if (TimeSpan.TryParse(ReservationTime, out var time))
            dto.ReservationDate = dto.ReservationDate.Date + time;
        dto.ReservationStatus = 0;
        dto.ReservationIsRead = false;
        dto.ReservationCreateDate = DateTime.Now;
        HttpClient client = _httpClientFactory.CreateClient("RestaurantApiClient");
        HttpResponseMessage response = await client.PostAsJsonAsync("api/Reservations", dto);
        if (response.IsSuccessStatusCode)
        {
            return Json(new
            {
                success = true,
                message = "👌 Rezervasyon talebiniz alındı, en kısa sürede dönüş yapılacaktır."
            });
        }
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            string? content = await response.Content.ReadAsStringAsync();
            try
            {
                ValidationProblemDetails? problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content);
                List<string>? apiErrors = problemDetails?.Errors?.SelectMany(x => x.Value).ToList()
                                 ?? new List<string> { "Geçersiz istek" };
                return Json(new { success = false, errors = apiErrors });
            }
            catch
            {
                return Json(new { success = false, errors = new List<string> { content } });
            }
        }
        return Json(new
        {
            success = false,
            errors = new List<string> { "Sunucu hatası oluştu. Lütfen tekrar deneyin." }
        });
    }
}