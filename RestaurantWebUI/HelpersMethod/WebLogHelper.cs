using System.IO;
using System.Text.Json;

namespace RestaurantWebUI.HelpersMethod
{
    public static class WebLogHelper
    {
        public static string GetUserIp(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString() ?? "IP Bulunamadı";
        }
        public static string GetUserAgent(HttpContext context)
        {
            return context.Request.Headers["User-Agent"].ToString();
        }
        public static async Task<string> GetGeoInfoAsync(string ipAddress)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                string url = $"https://ipapi.co/{ipAddress}/json/";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return $"Lokasyon alınamadı (IP: {ipAddress})";
                string content = await response.Content.ReadAsStringAsync();
                var geoData = JsonSerializer.Deserialize<IpApiResult>(content);
                if (geoData == null)
                    return $"Lokasyon alınamadı (IP: {ipAddress})";
                List<string> parts = new List<string>
        {
            geoData.City,
            geoData.Region,
            geoData.CountryName,
            geoData.ContinentCode
        };

                string location = string.Join(" - ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
                return location;
            }
            catch
            {
                return $"Lokasyon alınamadı (IP: {ipAddress})";
            }
        }
        private class IpApiResult
        {
            public string City { get; set; }
            public string Region { get; set; }
            public string CountryName { get; set; }
            public string ContinentCode { get; set; }
        }
    }
}