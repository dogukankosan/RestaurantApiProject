using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.GalleryImageDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryImagesController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public GalleryImagesController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GalleryImage> images = await _context.GalleryImages
                .AsNoTracking()
                .ToListAsync();
            List<ResultGalleryImageDto> result = _mapper.Map<List<ResultGalleryImageDto>>(images);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            GalleryImage? image = await _context.GalleryImages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ImageID == id);
            if (image is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Resim Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen bir resim bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            ResultGalleryImageDto result = _mapper.Map<ResultGalleryImageDto>(image);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateGalleryImageDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            GalleryImage? entity = _mapper.Map<GalleryImage>(dto);
            try
            {
                await _context.GalleryImages.AddAsync(entity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = entity.ImageID }, null);
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGalleryImageDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (id != dto.ImageID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = $"URL'deki ID ({id}) ile DTO içindeki ID ({dto.ImageID}) uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            GalleryImage? entity = await _context.GalleryImages.FindAsync(id);
            if (entity is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Resim Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen resim bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            _mapper.Map(dto, entity);
            try
            {
                _context.GalleryImages.Update(entity);
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
            GalleryImage? entity = await _context.GalleryImages.FindAsync(id);
            if (entity is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Resim Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen silinecek resim bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            try
            {
                _context.GalleryImages.Remove(entity);
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
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateGalleryImageStatusDto dto)
        {
            if (id != dto.ImageID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "URL ID ile DTO ID uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            GalleryImage? gallery = await _context.GalleryImages.FindAsync(id);
            if (gallery is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Resim Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen resim bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            if (gallery.ImageStatus == dto.ImageStatus)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Durum Zaten Güncel",
                    Detail = $"Şef zaten {(dto.ImageStatus ? "aktif" : "pasif")} durumda.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            gallery.ImageStatus = dto.ImageStatus;
            try
            {
                _context.GalleryImages.Update(gallery);
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