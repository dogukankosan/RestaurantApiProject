using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.CompanyInfoDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyInfosController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public CompanyInfosController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                CompanyInfo? companyInfo = await _context.CompanyInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (companyInfo == null)
                    return NotFound(new { Message = "Şirket bilgileri bulunamadı." });
                ResultCompanyInfoDto result = _mapper.Map<ResultCompanyInfoDto>(companyInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CompanyInfo-Get] Hata: {ex.Message}");
                return StatusCode(500, new { Message = "Sunucu hatası oluştu." });
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyInfoDto dto)
        {
            dto.CompanyInfoWebSiteLink ??= string.Empty;
            dto.CompanyInfoGithubLink ??= string.Empty;
            dto.CompanyInfoInstagramLink ??= string.Empty;
            dto.CompanyInfoLinkedinLink ??= string.Empty;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                CompanyInfo? entity = await _context.CompanyInfos.FirstOrDefaultAsync();
                if (entity == null)
                    return NotFound(new { Message = "Güncellenecek şirket bilgisi bulunamadı." });
                _mapper.Map(dto, entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Şirket bilgileri başarıyla güncellendi." });
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"[CompanyInfo-Update] DB Hatası: {dbEx.Message}");
                return StatusCode(500, new { Message = "Veritabanı güncelleme sırasında bir hata oluştu." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CompanyInfo-Update] Genel Hata: {ex.Message}");
                return StatusCode(500, new { Message = "Sunucu hatası oluştu." });
            }
        }
    }
}