using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.ProductDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public ProductsController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Product> products = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
            List<ResultProductDto> result = _mapper.Map<List<ResultProductDto>>(products);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Product? product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductID == id);
            if (product == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Ürün Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen bir ürün bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            ResultProductDto result = _mapper.Map<ResultProductDto>(product);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            dto.ProductDescription ??= string.Empty;
            Product? entity = _mapper.Map<Product>(dto);
            try
            {
                await _context.Products.AddAsync(entity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = entity.ProductID }, null);
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (id != dto.ProductID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = $"URL ID ({id}) ile DTO ID ({dto.ProductID}) uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Product? entity = await _context.Products.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Ürün Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen bir ürün bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            dto.ProductDescription ??= string.Empty;
            _mapper.Map(dto, entity);
            try
            {
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
                return NoContent();
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
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product? entity = await _context.Products.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Ürün Bulunamadı",
                    Detail = $"ID: {id} ile silinecek bir ürün bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            try
            {
                _context.Products.Remove(entity);
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
        [HttpPatch("{id}/Status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateProductStatusDto dto)
        {
            if (id != dto.ProductID)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Uyuşmazlığı",
                    Detail = "Gönderilen ID ile DTO içindeki ID uyuşmuyor.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Ürün Bulunamadı",
                    Detail = $"ID: {id} ile eşleşen ürün bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            product.ProductStatus = dto.ProductStatus;
            try
            {
                _context.Products.Update(product);
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