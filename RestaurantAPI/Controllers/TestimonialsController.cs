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
                return NotFound("Referans bulunamadı.");
            ResultTestimonialDto result = _mapper.Map<ResultTestimonialDto>(testimonial);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTestimonialDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Testimonial entity = _mapper.Map<Testimonial>(dto);
            await _context.Testimonials.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Referans ekleme işlemi başarılı");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTestimonialDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.TestimonialID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            Testimonial? exists = await _context.Testimonials
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TestimonialID == id);
            if (exists == null)
                return NotFound("Referans bulunamadı.");
            Testimonial entity = await _context.Testimonials.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Referans güncelleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Testimonial? entity = await _context.Testimonials.FindAsync(id);
            if (entity == null)
                return NotFound("Referans bulunamadı.");
            _context.Testimonials.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Referans silme işlemi başarılı");
        }
    }
}