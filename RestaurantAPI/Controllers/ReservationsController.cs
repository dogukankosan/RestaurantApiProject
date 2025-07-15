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
            List<Reservation>? reservations = await _context.Reservations
                                             .AsNoTracking()
                                             .OrderByDescending(x => x.ReservationCreateDate)
                                             .ToListAsync();
            List<ResultReservationDto>? result = _mapper.Map<List<ResultReservationDto>>(reservations);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Reservation? reservation = await _context.Reservations
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.ReservationID == id);
            if (reservation == null)
                return NotFound(new { message = "Rezervasyon bulunamadı." });
            ResultReservationDto? result = _mapper.Map<ResultReservationDto>(reservation);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Reservation? entity = _mapper.Map<Reservation>(dto);
                await _context.Reservations.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Rezervasyon başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Sunucu hatası oluştu.", detail = ex.Message });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Reservation? entity = await _context.Reservations.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Rezervasyon bulunamadı." });
            try
            {
                _context.Reservations.Remove(entity);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Rezervasyon başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Silme işlemi sırasında hata oluştu.", detail = ex.Message });
            }
        }
        [HttpPatch("UpdateStatus/{id:int}")]
        public async Task<IActionResult> UpdateReservationStatus(int id, [FromBody] UpdateReservationStatusDto dto)
        {
            if (id != dto.ReservationID)
                return BadRequest(new { message = "ID uyuşmazlığı. Lütfen sayfayı yenileyin." });
            if (dto.ReservationStatus < 0 || dto.ReservationStatus > 4)
                return BadRequest(new { message = "Geçersiz rezervasyon durumu. Yalnızca 0-4 arası değerler kabul edilir." });
            Reservation? reservation = await _context.Reservations.FindAsync(id);
            if (reservation is null)
                return NotFound(new { message = "Rezervasyon bulunamadı." });
            reservation.ReservationStatus = dto.ReservationStatus;
            reservation.ReservationIsRead = true;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Rezervasyon durumu başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Durum güncelleme sırasında hata oluştu.", detail = ex.Message });
            }
        }
        [HttpPatch("ToggleReadStatus/{id:int}")]
        public async Task<IActionResult> ToggleReadStatus(int id)
        {
            try
            {
                Reservation? reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null)
                    return NotFound(new { message = "Rezervasyon bulunamadı." });
                reservation.ReservationIsRead = !reservation.ReservationIsRead;
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = $"Rezervasyon {(reservation.ReservationIsRead ? "okundu" : "bekliyor")} olarak işaretlendi.",
                    newStatus = reservation.ReservationIsRead
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Okunma durumu güncellenirken hata oluştu.",
                    detail = ex.Message
                });
            }
        }
    }
}