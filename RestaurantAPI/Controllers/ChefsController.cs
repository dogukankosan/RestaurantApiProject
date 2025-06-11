using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.ChefDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public ChefsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Chef> chefs = await _context.Chefs
                .AsNoTracking()
                .ToListAsync();
            List<ResultChefDto> result = _mapper.Map<List<ResultChefDto>>(chefs);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Chef? chef = await _context.Chefs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ChefID == id);
            if (chef == null)
                return NotFound("Şef bulunamadı.");
            ResultChefDto result = _mapper.Map<ResultChefDto>(chef);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateChefDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Chef entity = _mapper.Map<Chef>(dto);
            await _context.Chefs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Şef ekleme işlemi başarılı");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateChefDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.ChefID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            Chef? exists = await _context.Chefs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ChefID == id);
            if (exists == null)
                return NotFound("Şef bulunamadı.");
            Chef entity = await _context.Chefs.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Şef güncelleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Chef? entity = await _context.Chefs.FindAsync(id);
            if (entity == null)
                return NotFound("Şef bulunamadı.");
            _context.Chefs.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Şef silme işlemi başarılı");
        }
    }
}