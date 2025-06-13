using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.CategoryDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public CategoriesController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Category> categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();
            List<ResultCategoryDto> result = _mapper.Map<List<ResultCategoryDto>>(categories);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Category? category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryID == id);
            if (category == null)
                return NotFound("Kategori bulunamadı");
            GetByIDCategoryDto result = _mapper.Map<GetByIDCategoryDto>(category);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Category entity = _mapper.Map<Category>(dto);
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Kategori ekleme işlemi başarılı");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.CategoryID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            Category? exists = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryID == id);
            if (exists == null)
                return NotFound("Kategori bulunamadı");
            Category entity = await _context.Categories.FindAsync(id)!;
            _mapper.Map(dto, entity);
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
            return Ok("Kategori güncelleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category? entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return NotFound("Kategori bulunamadı");
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Kategori silme işlemi başarılı");
        }
    }
}