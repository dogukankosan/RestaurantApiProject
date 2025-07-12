using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.ChefDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;

        public ChefsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Chef> chefs = await _context.Chefs
                .AsNoTracking()
                .ToListAsync();
            List<ResultChefDto> result = _mapper.Map<List<ResultChefDto>>(chefs);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Chef? chef = await _context.Chefs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ChefID == id);
            if (chef is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Şef Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen şef kaydı bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            ResultChefDto result = _mapper.Map<ResultChefDto>(chef);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateChefDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            dto.ChefDescription ??= "";
            dto.ChefTwitterLink ??= "";
            dto.ChefFacebookLink ??= "";
            dto.ChefInstagramLink ??= "";
            dto.ChefLinkedinLink ??= "";
            Chef? entity = _mapper.Map<Chef>(dto);
            try
            {
                await _context.Chefs.AddAsync(entity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = entity.ChefID }, null);
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateChefDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (id != dto.ChefID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = $"URL'deki ID ({id}) ile DTO içindeki ID ({dto.ChefID}) uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Chef? entity = await _context.Chefs.FindAsync(id);
            if (entity is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Şef Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen şef kaydı bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            dto.ChefDescription ??= "";
            dto.ChefTwitterLink ??= "";
            dto.ChefFacebookLink ??= "";
            dto.ChefInstagramLink ??= "";
            dto.ChefLinkedinLink ??= "";
            _mapper.Map(dto, entity);
            try
            {
                _context.Chefs.Update(entity);
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
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Chef? entity = await _context.Chefs.FindAsync(id);
            if (entity is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Şef Bulunamadı",
                    Detail = $"ID: {id} ile silinecek şef bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            try
            {
                _context.Chefs.Remove(entity);
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
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateChefStatusDto dto)
        {
            if (id != dto.ChefID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "URL ID ile DTO ID uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Chef? chef = await _context.Chefs.FindAsync(id);
            if (chef is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Şef Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen şef bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            if (chef.ChefStatus == dto.IsActive)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Durum Zaten Güncel",
                    Detail = $"Şef zaten {(dto.IsActive ? "aktif" : "pasif")} durumda.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            chef.ChefStatus = dto.IsActive;
            try
            {
                _context.Chefs.Update(chef);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Durum Güncelleme Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}