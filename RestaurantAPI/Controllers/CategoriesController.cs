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
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Kategori Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen bir kategori bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            GetByIDCategoryDto result = _mapper.Map<GetByIDCategoryDto>(category);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            Category? entity = _mapper.Map<Category>(dto);
            try
            {
                await _context.Categories.AddAsync(entity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = entity.CategoryID }, null);
            }
            catch (DbUpdateException ex)
            {
                // TODO: Logla (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Veritabanı Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (id != dto.CategoryID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = $"URL ID ({id}) ile DTO ID ({dto.CategoryID}) uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Category? entity = await _context.Categories.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Kategori Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen kategori bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            _mapper.Map(dto, entity);
            try
            {
                _context.Categories.Update(entity);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // TODO: Logla (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Veritabanı Güncelleme Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category? entity = await _context.Categories.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Kategori Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen kategori bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            bool hasProducts = await _context.Products.AnyAsync(p => p.CategoryID == id);
            if (hasProducts)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Kategori Silinemedi",
                    Detail = "Bu kategoriye ait ürünler mevcut. Önce ürünleri silin ya da başka kategoriye taşıyın.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            try
            {
                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // TODO: Logla (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Silme Hatası",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpPatch("{id}/Status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateCategoryStatusDto dto)
        {
            if (id != dto.CategoryID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = $"URL ID ({id}) ile DTO ID ({dto.CategoryID}) uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Kategori Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen kategori bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            category.CategoryStatus = dto.CategoryStatus;
            try
            {
                _context.Categories.Update(category);
                List<Product> relatedProducts = await _context.Products
                    .Where(p => p.CategoryID == id)
                    .ToListAsync();
                foreach (Product product in relatedProducts)
                {
                    product.ProductStatus = dto.CategoryStatus;
                    _context.Products.Update(product);
                }
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // TODO: Logla (ex)
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