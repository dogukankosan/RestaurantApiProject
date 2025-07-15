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
            About? about = await _context.Abouts.AsNoTracking().FirstOrDefaultAsync();
            if (about == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Hakkında Kaydı Bulunamadı",
                    Detail = "Veritabanında herhangi bir 'About' kaydı bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            ResultAboutDto? result = _mapper.Map<ResultAboutDto>(about);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAboutDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            About? about = await _context.Abouts.FirstOrDefaultAsync();
            if (about == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Güncelleme Başarısız",
                    Detail = "Güncellenecek 'About' kaydı bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            _mapper.Map(dto, about);
            try
            {
                _context.Abouts.Update(about);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                //TODO: Loglama yapılmalı (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Veritabanı Hatası",
                    Detail = "Veri kaydedilirken bir hata oluştu.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            catch (Exception ex)
            {
                //TODO: Loglama yapılmalı (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Sunucu Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}