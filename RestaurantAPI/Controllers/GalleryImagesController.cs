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
            if (image == null)
                return NotFound("Resim bulunamadı.");
            ResultGalleryImageDto result = _mapper.Map<ResultGalleryImageDto>(image);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateGalleryImageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            GalleryImage entity = _mapper.Map<GalleryImage>(dto);
            await _context.GalleryImages.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Resim ekleme işlemi başarılı");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGalleryImageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.ImageID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            GalleryImage? exists = await _context.GalleryImages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ImageID == id);
            if (exists == null)
                return NotFound("Resim bulunamadı.");
            GalleryImage entity = await _context.GalleryImages.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Resim güncelleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            GalleryImage? entity = await _context.GalleryImages.FindAsync(id);
            if (entity == null)
                return NotFound("Resim bulunamadı.");
            _context.GalleryImages.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Resim silme işlemi başarılı");
        }
    }
}