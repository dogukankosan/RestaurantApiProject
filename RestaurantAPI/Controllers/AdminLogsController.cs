using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.AdminLogDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLogsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public AdminLogsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<AdminLog> logs = await _context.AdminLogs
                .AsNoTracking()
                .ToListAsync();
            List<ResultAdminLogDto> result = _mapper.Map<List<ResultAdminLogDto>>(logs);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            AdminLog? logs = await _context.AdminLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LogID == id);
            if (logs == null)
                return NotFound("Hata log bulunamadı.");
            ResultAdminLogDto result = _mapper.Map<ResultAdminLogDto>(logs);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAdminLogDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            AdminLog entity = _mapper.Map<AdminLog>(dto);
            await _context.AdminLogs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Log ekleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            AdminLog? entity = await _context.AdminLogs.FindAsync(id);
            if (entity == null)
                return NotFound("Log bulunamadı.");
            _context.AdminLogs.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Log silme işlemi başarılı");
        }
    }
}