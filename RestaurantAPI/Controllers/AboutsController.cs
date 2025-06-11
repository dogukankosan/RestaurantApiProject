using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.AboutDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public AboutsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            About abouts = await _context.Abouts.FirstOrDefaultAsync();
            if (abouts == null)
                return NotFound("Hakkında kaydı bulunamadı");
            ResultAboutDto result = _mapper.Map<ResultAboutDto>(abouts);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAboutDto dto)
        {
            if (id != dto.AboutID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            var exists = await _context.Abouts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AboutID == id);
            if (exists == null)
                return NotFound("Hakkında kaydı bulunamadı.");
            var entity = await _context.Abouts.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Hakkında güncelleme işlemi başarılı");
        }
    }
}