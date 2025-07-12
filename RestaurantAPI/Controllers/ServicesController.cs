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
            List<Service> services = await _context.Services.AsNoTracking().ToListAsync();
            List<ResultServiceDto> result = _mapper.Map<List<ResultServiceDto>>(services);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Service?  service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(x => x.ServiceID == id);
            if (service is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Servis Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen servis bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            ResultServiceDto result = _mapper.Map<ResultServiceDto>(service);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateServiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Service? entity = _mapper.Map<Service>(dto);
            await _context.Services.AddAsync(entity);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = entity.ServiceID }, _mapper.Map<ResultServiceDto>(entity));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Veritabanı Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.ServiceID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "URL'deki ID ile gövde verisi uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Service? entity = await _context.Services.FindAsync(id);
            if (entity is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Servis Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen servis bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            _mapper.Map(dto, entity);
            _context.Services.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Servis başarıyla güncellendi.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Güncelleme Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Service? entity = await _context.Services.FindAsync(id);
            if (entity is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Servis Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen servis bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            _context.Services.Remove(entity);
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Silme Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpPatch("UpdateStatus/{id:int}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateServiceStatusDto dto)
        {
            if (id != dto.ServiceID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "URL'deki ID ile gönderilen verideki ID eşleşmiyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Service? service = await _context.Services.FindAsync(id);
            if (service is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Servis Bulunamadı",
                    Detail = $"ID'si {id} olan servis bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            if (service.ServiceStatus == dto.IsActive)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Zaten Güncel",
                    Detail = $"Servis zaten {(dto.IsActive ? "aktif" : "pasif")} durumda.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            service.ServiceStatus = dto.IsActive;
            try
            {
                _context.Services.Update(service);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Güncelleme Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}