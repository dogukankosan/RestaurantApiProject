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
    public class FeatureController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly APIContext _context;
        public FeatureController(IMapper mapper, APIContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Feature> features = await _context.Features.AsNoTracking().ToListAsync();
            List<ResultFeatureDto> result = _mapper.Map<List<ResultFeatureDto>>(features);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Feature? feature = await _context.Features.FindAsync(id);
            if (feature == null)
                return NotFound("Özellik bulunamadı.");
            GetByIDFeatureDto result = _mapper.Map<GetByIDFeatureDto>(feature);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateFeatureDto dto)
        {
            Feature entity = _mapper.Map<Feature>(dto);
            await _context.Features.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Özellik ekleme işlemi başarılı");
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFeatureDto dto)
        {
            Feature? exists = await _context.Features
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FeatureID == dto.FeatureID);
            if (exists == null)
                return NotFound("Özellik bulunamadı.");
            Feature updatedEntity = _mapper.Map<Feature>(dto);
            _context.Entry(updatedEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Özellik güncelleme işlemi başarılı");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Feature? entity = await _context.Features.FindAsync(id);
            if (entity == null)
                return NotFound("Özellik bulunamadı.");
            _context.Features.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Özellik silme işlemi başarılı");
        }
    }
}