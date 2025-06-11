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
            CompanyInfo? companyInfo = await _context.CompanyInfos
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (companyInfo == null)
                return NotFound("Şirket bilgileri bulunamadı");
            ResultCompanyInfoDto result = _mapper.Map<ResultCompanyInfoDto>(companyInfo);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyInfoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.CompanyInfoID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            CompanyInfo? exists = await _context.CompanyInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CompanyInfoID == id);
            if (exists == null)
                return NotFound("Şirket bilgileri bulunamadı");
            CompanyInfo entity = await _context.CompanyInfos.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Şirket bilgileri güncelleme işlemi başarılı");
        }
    }
}