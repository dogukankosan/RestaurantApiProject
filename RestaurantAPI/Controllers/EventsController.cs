using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.EventDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public EventsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultEventDto>>> GetAll()
        {
            List<Event>? events = await _context.Events.AsNoTracking().ToListAsync();
            List<ResultEventDto>? result = _mapper.Map<List<ResultEventDto>>(events);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Event? evt = await _context.Events.AsNoTracking().FirstOrDefaultAsync(x => x.EventID == id);
            if (evt is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Etkinlik Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen etkinlik bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            GetByIDEventDto? result = _mapper.Map<GetByIDEventDto>(evt);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEventDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Event? entity = _mapper.Map<Event>(dto);
                await _context.Events.AddAsync(entity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = entity.EventID }, "Etkinlik başarıyla eklendi");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Etkinlik Ekleme Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventDto dto)
        {
            if (id != dto.EventID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "Gönderilen ID ile DTO içindeki ID eşleşmiyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            bool exists = await _context.Events.AsNoTracking().AnyAsync(x => x.EventID == id);
            if (!exists)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Etkinlik Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen etkinlik yok.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            try
            {
                Event? entity = await _context.Events.FindAsync(id);
                _mapper.Map(dto, entity);
                _context.Events.Update(entity);
                await _context.SaveChangesAsync();
                return Ok("Etkinlik başarıyla güncellendi");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Veritabanı Güncelleme Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Event? entity = await _context.Events.FindAsync(id);
            if (entity is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Etkinlik Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen etkinlik yok.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            try
            {
                _context.Events.Remove(entity);
                await _context.SaveChangesAsync();
                return Ok("Etkinlik başarıyla silindi");
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
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateEventStatusDto dto)
        {
            if (id != dto.EventID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "URL'deki ID ile gönderilen verideki ID eşleşmiyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Event? evt = await _context.Events.FindAsync(id);
            if (evt is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Etkinlik Bulunamadı",
                    Detail = $"ID'si {id} olan etkinlik sistemde mevcut değil.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            if (evt.EventStatus == dto.IsActive)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Zaten Güncel",
                    Detail = $"Etkinlik zaten {(dto.IsActive ? "aktif" : "pasif")} durumda.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            evt.EventStatus = dto.IsActive;
            try
            {
                _context.Events.Update(evt);
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