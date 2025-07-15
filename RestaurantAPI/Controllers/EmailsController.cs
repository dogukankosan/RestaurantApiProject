using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.EmailDtos;
using RestaurantAPI.Entities;
using RestaurantAPI.Helpers;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public EmailsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetEmailSettings()
        {
            Email? email = await _context.Emails.AsNoTracking().FirstOrDefaultAsync();
            if (email == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "E-posta Ayarı Bulunamadı",
                    Detail = "Veritabanında herhangi bir e-posta ayarı kaydı bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            ResultEmailDto result = _mapper.Map<ResultEmailDto>(email);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateEmailSettings([FromForm] UpdateEmailDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            Email? email = await _context.Emails.FirstOrDefaultAsync();
            if (email == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Güncelleme Başarısız",
                    Detail = "Güncellenecek e-posta kaydı bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            bool testSent = await EmailSender.TrySendTestEmailAsync(dto, "Test Maili", "Sistem doğrulaması için bu test maili gönderildi.");
            if (!testSent)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Test Mail Gönderilemedi",
                    Detail = "Lütfen mail sunucu ayarlarını (şifre, sunucu, port, SSL) kontrol edin.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            try
            {
                dto.EmailPassword = EncryptionHelper.Encrypt(dto.EmailPassword);
                dto.EmailCompanyName = dto.EmailCompanyName?.Trim() ?? string.Empty;
                dto.EmailAddress = dto.EmailAddress?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(dto.EmailPhone) || dto.EmailPhone.Length <= 5)
                    dto.EmailPhone = string.Empty;
                _mapper.Map(dto, email);
                _context.Emails.Update(email);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Veritabanı Hatası",
                    Detail = "Veri kaydedilirken bir hata oluştu.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Sunucu Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpPut("Reset")]
        public async Task<IActionResult> ResetToDefault()
        {
            Email? email = await _context.Emails.FirstOrDefaultAsync();
            if (email == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Sıfırlama Başarısız",
                    Detail = "Sıfırlanacak e-posta kaydı bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            try
            {
                email.EmailBox = "";
                email.EmailAddress = "";
                email.EmailPassword = "";
                email.EmailServer = "";
                email.EmailPort = 0;
                email.EmailSSl = 0;
                email.EmailCompanyName = "";
                email.EmailPhone = "";
                email.EmailImage = null;
                _context.Emails.Update(email);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Sunucu Hatası",
                    Detail = $"Sıfırlama işlemi sırasında hata oluştu: {ex.Message}",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}