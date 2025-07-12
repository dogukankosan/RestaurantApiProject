using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.TestimonialDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public TestimonialsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Testimonial> testimonials = await _context.Testimonials
                .AsNoTracking()
                .ToListAsync();
            List<ResultTestimonialDto> result = _mapper.Map<List<ResultTestimonialDto>>(testimonials);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Testimonial? testimonial = await _context.Testimonials
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TestimonialID == id);
            if (testimonial == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Referans Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen referans bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            ResultTestimonialDto result = _mapper.Map<ResultTestimonialDto>(testimonial);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTestimonialDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            Testimonial? entity = _mapper.Map<Testimonial>(dto);
            try
            {
                await _context.Testimonials.AddAsync(entity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = entity.TestimonialID }, null);
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTestimonialDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (id != dto.TestimonialID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = $"URL ID ({id}) ile DTO ID ({dto.TestimonialID}) farklı.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Testimonial? entity = await _context.Testimonials.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Referans Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen referans bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            _mapper.Map(dto, entity);
            try
            {
                _context.Testimonials.Update(entity);
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
            Testimonial? entity = await _context.Testimonials.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Referans Bulunamadı",
                    Detail = $"ID: {id} ile silinecek bir referans bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            try
            {
                _context.Testimonials.Remove(entity);
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
        [HttpPatch("UpdateStatus/{id:int}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTestimonialStatusDto dto)
        {
            if (id != dto.TestimonialID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "URL ID ile DTO ID uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Testimonial? testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Referans Bulunamadı",
                    Detail = $"ID: {id} numaralı referans bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            if (testimonial.TestimonialStatus == dto.IsActive)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Durum Zaten Güncel",
                    Detail = $"Referans zaten {(dto.IsActive ? "aktif" : "pasif")} durumda.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            testimonial.TestimonialStatus = dto.IsActive;
            try
            {
                _context.Testimonials.Update(testimonial);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Güncelleme Hatası",
                    Detail = "Durum güncellenirken hata oluştu: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}