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
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Özellik Bulunamadı",
                    Detail = "Veritabanında kayıtlı bir özellik bulunamadı.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            ResultFeatureDto result = _mapper.Map<ResultFeatureDto>(feature);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFeatureDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            Feature? exists = await _context.Features
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (exists == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Özellik Bulunamadı",
                    Status = StatusCodes.Status404NotFound
                });
            }
            try
            {
                Feature? entity = await _context.Features.FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Özellik Bulunamadı",
                        Status = StatusCodes.Status404NotFound
                    });
                }
                _mapper.Map(dto, entity);
                _context.Features.Update(entity);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Özellik başarıyla güncellendi.",
                    UpdatedFeatureID = entity.FeatureID
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Veritabanı Hatası",
                    Detail = ex.InnerException?.Message ?? ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}