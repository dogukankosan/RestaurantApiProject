using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.FeatureDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public FeaturesController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Feature? feature = await _context.Features
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (feature == null)
                return NotFound("Özellik bulunamadı.");
            ResultFeatureDto result = _mapper.Map<ResultFeatureDto>(feature);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFeatureDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.FeatureID)
                return BadRequest("Gönderilen ID ile DTO içindeki ID eşleşmiyor.");
            Feature? exists = await _context.Features
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FeatureID == id);
            if (exists == null)
                return NotFound("Özellik bulunamadı.");
            Feature entity = await _context.Features.FindAsync(id)!;
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return Ok("Özellik güncelleme işlemi başarılı");
        }
    }
}