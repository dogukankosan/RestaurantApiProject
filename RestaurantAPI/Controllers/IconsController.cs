using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.IconDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IconsController : ControllerBase
    {
        private readonly APIContext _context;

        public IconsController(APIContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Icon> icons = await _context.Icons.AsNoTracking().ToListAsync();
            return Ok(icons);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Icon? icon = await _context.Icons.AsNoTracking().FirstOrDefaultAsync(x => x.IconID == id);
            if (icon == null)
                return NotFound(new ProblemDetails { Title = "Bulunamadı", Detail = "İkon bulunamadı.", Status = 404 });
            return Ok(icon);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateIconDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Icon icon = new Icon
            {
                IconURL = dto.IconURL,
                IconStatus = dto.IconStatus
            };
            await _context.Icons.AddAsync(icon);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "İkon eklendi.", icon.IconID });
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateIconDto dto)
        {
            if (!ModelState.IsValid || id != dto.IconID)
                return BadRequest(new ProblemDetails { Title = "Geçersiz Veri", Detail = "ID uyuşmuyor veya model hatalı.", Status = 400 });
            Icon? icon = await _context.Icons.FindAsync(id);
            if (icon == null)
                return NotFound(new ProblemDetails { Title = "Bulunamadı", Detail = "İkon bulunamadı.", Status = 404 });
            icon.IconURL = dto.IconURL;
            icon.IconStatus = dto.IconStatus;
            try
            {
                _context.Icons.Update(icon);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "İkon güncellendi." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Veritabanı Hatası", Detail = ex.Message, Status = 500 });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Icon? icon = await _context.Icons.FindAsync(id);
            if (icon == null)
                return NotFound(new ProblemDetails { Title = "Bulunamadı", Detail = "İkon bulunamadı.", Status = 404 });
            _context.Icons.Remove(icon);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "İkon silindi." });
        }
        [HttpPatch("UpdateStatus/{id:int}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateIconStatusDto dto)
        {
            if (id != dto.IconID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "URL ID ile DTO ID eşleşmiyor.",
                    Status = 400
                });
            }
            Icon? icon = await _context.Icons.FindAsync(id);
            if (icon == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "İkon Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen ikon bulunamadı.",
                    Status = 404
                });
            }
            if (icon.IconStatus == dto.IconStatus)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Durum Zaten Güncel",
                    Detail = $"İkon zaten {(dto.IconStatus ? "aktif" : "pasif")} durumda.",
                    Status = 400
                });
            }
            icon.IconStatus = dto.IconStatus;
            try
            {
                _context.Icons.Update(icon);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Durum Güncelleme Hatası",
                    Detail = ex.Message,
                    Status = 500
                });
            }
        }
    }
}