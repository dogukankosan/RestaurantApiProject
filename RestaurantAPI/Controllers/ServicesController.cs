using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.ServiceDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public ServicesController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Service> services = await _context.Services
                .AsNoTracking()
                .ToListAsync();
            List<ResultServiceDto> result = _mapper.Map<List<ResultServiceDto>>(services);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Service? service = await _context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServiceID == id);
            if (service == null)
                return NotFound("Servis bulunamadı.");
            ResultServiceDto result = _mapper.Map<ResultServiceDto>(service);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateServiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Service entity = _mapper.Map<Service>(dto);
            await _context.Services.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Servis ekleme işlemi başarılı");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.ServiceID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            Service? exists = await _context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServiceID == id);
            if (exists == null)
                return NotFound("Servis bulunamadı.");
            Service entity = await _context.Services.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Servis güncelleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Service? entity = await _context.Services.FindAsync(id);
            if (entity == null)
                return NotFound("Servis bulunamadı.");
            _context.Services.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Servis silme işlemi başarılı");
        }
    }
}