using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.WebLogDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebLogsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public WebLogsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<WebLog> logs = await _context.WebLogs.AsNoTracking().ToListAsync();
                List<ResultWebLogDto> result = _mapper.Map<List<ResultWebLogDto>>(logs);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Sunucu Hatası",
                    Detail = "Web log kayıtları alınırken bir hata oluştu.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWebLogDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Geçersiz Veri",
                    Detail = "Geçerli bir veri gönderilmelidir.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            try
            {
                WebLog entity = _mapper.Map<WebLog>(dto);
                await _context.WebLogs.AddAsync(entity);
                await _context.SaveChangesAsync();
                ResultWebLogDto result = _mapper.Map<ResultWebLogDto>(entity);
                return Created($"/api/weblogs", result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Sunucu Hatası",
                    Detail = "Web log eklenirken bir hata oluştu.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                WebLog? entity = await _context.WebLogs.FindAsync(id);
                if (entity == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Silme Başarısız",
                        Detail = $"ID'si {id} olan web log kaydı bulunamadı.",
                        Status = StatusCodes.Status404NotFound
                    });
                }
                _context.WebLogs.Remove(entity);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Sunucu Hatası",
                    Detail = "Silme işlemi sırasında bir hata oluştu.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}