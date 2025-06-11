using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.MessageDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public MessagesController(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Message> messages = await _context.Messages
                .AsNoTracking()
                .ToListAsync();
            List<ResultMessageDto> result = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Message? message = await _context.Messages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MessageID == id);
            if (message == null)
                return NotFound("Mesaj bulunamadı.");
            ResultMessageDto result = _mapper.Map<ResultMessageDto>(message);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Message entity = _mapper.Map<Message>(dto);
            await _context.Messages.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok("Mesaj ekleme işlemi başarılı");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Message? entity = await _context.Messages.FindAsync(id);
            if (entity == null)
                return NotFound("Mesaj bulunamadı.");
            _context.Messages.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Mesaj silme işlemi başarılı");
        }
    }
}