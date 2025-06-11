using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.ReservationDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public ReservationsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Reservation> reservations = await _context.Reservations
                .AsNoTracking()
                .ToListAsync();
            List<ResultReservationDto> result = _mapper.Map<List<ResultReservationDto>>(reservations);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Reservation? reservation = await _context.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReservationID == id);
            if (reservation == null)
                return NotFound("Rezervasyon bulunamadı.");
            ResultReservationDto result = _mapper.Map<ResultReservationDto>(reservation);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Reservation entity = _mapper.Map<Reservation>(dto);
            await _context.Reservations.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Rezervasyon ekleme işlemi başarılı");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.ReservationID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            Reservation? exists = await _context.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReservationID == id);
            if (exists == null)
                return NotFound("Rezervasyon bulunamadı.");
            Reservation entity = await _context.Reservations.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Rezervasyon güncelleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Reservation? entity = await _context.Reservations.FindAsync(id);
            if (entity == null)
                return NotFound("Rezervasyon bulunamadı.");
            _context.Reservations.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Rezervasyon silme işlemi başarılı");
        }
    }
}