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
        public async Task<IActionResult> GetAll()
        {
            List<Event> events = await _context.Events
                .AsNoTracking()
                .ToListAsync();
            List<ResultEventDto> result = _mapper.Map<List<ResultEventDto>>(events);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Event? evt = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EventID == id);
            if (evt == null)
                return NotFound("Etkinlik bulunamadı");
            GetByIDEventDto result = _mapper.Map<GetByIDEventDto>(evt);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEventDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Event entity = _mapper.Map<Event>(dto);
            await _context.Events.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Etkinlik ekleme işlemi başarılı");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.EventID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            Event? exists = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EventID == id);
            if (exists == null)
                return NotFound("Etkinlik bulunamadı");
            Event entity = await _context.Events.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Etkinlik güncelleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Event? entity = await _context.Events.FindAsync(id);
            if (entity == null)
                return NotFound("Etkinlik bulunamadı");
            _context.Events.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Etkinlik silme işlemi başarılı");
        }
    }
}