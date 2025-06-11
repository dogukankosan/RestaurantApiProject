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
                .AsNoTracking()
                .ToListAsync();
            List<ResultProductDto> result = _mapper.Map<List<ResultProductDto>>(products);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Product? product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductID == id);
            if (product == null)
                return NotFound("Malzeme bulunamadı.");
            ResultProductDto result = _mapper.Map<ResultProductDto>(product);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Product entity = _mapper.Map<Product>(dto);
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Malzeme ekleme işlemi başarılı");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.ProductID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            Product? exists = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductID == id);
            if (exists == null)
                return NotFound("Malzeme bulunamadı.");
            Product entity = await _context.Products.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Malzeme güncelleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product? entity = await _context.Products.FindAsync(id);
            if (entity == null)
                return NotFound("Malzeme bulunamadı.");
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Malzeme silme işlemi başarılı");
        }
    }
}