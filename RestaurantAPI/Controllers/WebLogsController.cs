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
            List<WebLog> logs = await _context.WebLogs
                .AsNoTracking()
                .ToListAsync();
            List<ResultWebLogDto> result = _mapper.Map<List<ResultWebLogDto>>(logs);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            WebLog? logs = await _context.WebLogs.AsNoTracking()
                .FirstOrDefaultAsync(x => x.WebLogID == id);
            if (logs == null)
                return NotFound("Hata web log bulunamadı.");
            ResultWebLogDto result = _mapper.Map<ResultWebLogDto>(logs);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWebLogDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            WebLog entity = _mapper.Map<WebLog>(dto);
            await _context.WebLogs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Web log ekleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            WebLog? entity = await _context.WebLogs.FindAsync(id);
            if (entity == null)
                return NotFound("Web log bulunamadı.");
            _context.WebLogs.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Web log silme işlemi başarılı");
        }
    }
}