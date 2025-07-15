using RestaurantAPI.Dtos.EmailDtos;

using System.Net;
using System.Net.Mail;
using System.Text;

public static class EmailSender
{
    public static async Task<bool> TrySendTestEmailAsync(UpdateEmailDto dto, string subject, string body)
    {
        try
        {
            using SmtpClient client = new(dto.EmailServer, dto.EmailPort)
            {
                Credentials = new NetworkCredential(dto.EmailBox, dto.EmailPassword),
                EnableSsl = dto.EmailSSl == 1
            };
            MailMessage message = new()
            {
                From = new MailAddress(dto.EmailBox, dto.EmailCompanyName ?? "Test"),
                Subject = subject,
                IsBodyHtml = true
            };
            message.To.Add(dto.EmailBox);
            string logoCid = "logoImage";
            string htmlBody = BuildHtmlBody(dto, body, logoCid);
            AlternateView altView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, "text/html");
            if (dto.EmailImage is { Length: > 0 })
            {
                string? mime = GetMimeType(dto.EmailImage);
                LinkedResource logo = new LinkedResource(new MemoryStream(dto.EmailImage), mime)
                {
                    ContentId = logoCid,
                    TransferEncoding = System.Net.Mime.TransferEncoding.Base64,
                    ContentType = new System.Net.Mime.ContentType(mime)
                    {
                        Name = "logo.jpg" 
                    },
                    ContentLink = new Uri("cid:" + logoCid)
                };
                altView.LinkedResources.Add(logo);
            }
            message.AlternateViews.Add(altView);
            await client.SendMailAsync(message);
            return true;
        }
        catch
        {
            return false;
        }
    }
    private static string BuildHtmlBody(UpdateEmailDto dto, string content, string? imageCid = null)
    {
        StringBuilder sb = new();
        sb.AppendLine("<div style='font-family:Arial,sans-serif; font-size:14px;'>");
        sb.AppendLine($"<p>{content}</p>");
        bool hasSignatureInfo =
            !string.IsNullOrWhiteSpace(dto.EmailCompanyName) ||
            !string.IsNullOrWhiteSpace(dto.EmailPhone) ||
            !string.IsNullOrWhiteSpace(dto.EmailAddress) ||
            (dto.EmailImage != null && dto.EmailImage.Length > 10);
        if (hasSignatureInfo)
        {
            sb.AppendLine("<hr style='margin-top:20px;' />");
            sb.AppendLine("<p><strong>İletişim Bilgileri:</strong></p>");
            sb.AppendLine("<table style='font-size:13px;'>");
            if (!string.IsNullOrWhiteSpace(dto.EmailCompanyName))
                sb.AppendLine($"<tr><td>🏢 Firma:</td><td>{dto.EmailCompanyName}</td></tr>");
            if (!string.IsNullOrWhiteSpace(dto.EmailPhone))
            {
                string phonePlain = dto.EmailPhone.Replace(" ", "").Replace("+", "").Trim();
                string whatsappLink = $"https://wa.me/{phonePlain}";
                sb.AppendLine($"<tr><td>📞 Telefon:</td><td><a href='{whatsappLink}' style='color:#0d6efd; text-decoration:none;' target='_blank'>{dto.EmailPhone}</a></td></tr>");
            }
            if (!string.IsNullOrWhiteSpace(dto.EmailAddress))
                sb.AppendLine($"<tr><td>📍 Adres:</td><td>{dto.EmailAddress}</td></tr>");
            sb.AppendLine("</table>");
            if (!string.IsNullOrWhiteSpace(imageCid) && dto.EmailImage?.Length > 0)
            {
                sb.AppendLine($@"
                 <div style='margin-top:15px; text-align:left;'>
                        <img src='cid:{imageCid}' 
                             alt='Logo' 
                             title='Firma Logosu' 
                             style='max-height:120px; border-radius:8px; display:inline-block;' />
                    </div>");
            }
        }
   sb.AppendLine("</div>");
        return sb.ToString();
    }
    private static string GetMimeType(byte[] imageBytes)
    {
        if (imageBytes.Length >= 3 && imageBytes[0] == 0xFF && imageBytes[1] == 0xD8)
            return "image/jpeg";
        if (imageBytes.Length >= 8 &&
            imageBytes[0] == 0x89 && imageBytes[1] == 0x50 &&
            imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
            return "image/png";
        if (imageBytes.Length >= 12 &&
            imageBytes[0] == 0x52 && imageBytes[1] == 0x49 &&
            imageBytes[2] == 0x46 && imageBytes[3] == 0x46 &&
            imageBytes[8] == 0x57 && imageBytes[9] == 0x45 &&
            imageBytes[10] == 0x42 && imageBytes[11] == 0x50)
            return "image/webp";
        return "application/octet-stream";
    }
}