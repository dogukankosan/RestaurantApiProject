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
        public async Task<ActionResult<List<ResultMessageDto>>> GetAll()
        {
            try
            {
                List<Message>? messages = await _context.Messages
                    .AsNoTracking()
                    .OrderByDescending(x => x.MessageSendDate) 
                    .ToListAsync();
                List<ResultMessageDto> result = _mapper.Map<List<ResultMessageDto>>(messages);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Title = "Mesajlar getirilemedi",
                    Detail = ex.Message
                });
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResultMessageDto>> GetById(int id)
        {
            try
            {
                Message? message = await _context.Messages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.MessageID == id);
                if (message is null)
                    return NotFound(new { message = "Mesaj bulunamadı." });
                ResultMessageDto result = _mapper.Map<ResultMessageDto>(message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Title = "Mesaj getirilemedi",
                    Detail = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateMessageDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                             .SelectMany(v => v.Errors)
                             .Select(e => e.ErrorMessage)
                             .ToList();
                return BadRequest(new { success = false, errors });
            }
            try
            {
                Message entity = _mapper.Map<Message>(dto);
                entity.MessageSendDate = DateTime.Now;
                await _context.Messages.AddAsync(entity);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    errors = new List<string> { "Mesaj gönderilemedi: " + (ex.InnerException?.Message ?? ex.Message) }
                });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Message? entity = await _context.Messages.FindAsync(id);
                if (entity is null)
                    return NotFound(new { message = "Silinecek mesaj bulunamadı." });
                _context.Messages.Remove(entity);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Mesaj başarıyla silindi." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    Title = "Mesaj silme hatası",
                    Detail = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        [HttpPatch("ToggleReadStatus/{id:int}")]
        public async Task<IActionResult> ToggleReadStatus(int id)
        {
            try
            {
                Message? message = await _context.Messages.FindAsync(id);
                if (message is null)
                    return NotFound(new { message = "Mesaj bulunamadı." });
                message.MessageIsRead = !message.MessageIsRead;
                _context.Messages.Update(message);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Mesaj durumu güncellendi.",
                    isRead = message.MessageIsRead
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Title = "Mesaj durumu güncellenemedi",
                    Detail = ex.Message
                });
            }
        }
        [HttpPatch("MarkAllAsRead")]
        public async Task<IActionResult> MarkAllMessagesAsRead()
        {
            try
            {
                List<Message>? unreadMessages = await _context.Messages
                    .Where(x => !x.MessageIsRead)
                    .ToListAsync();
                if (!unreadMessages.Any())
                    return Ok(new { message = "Zaten tüm mesajlar okunmuş." });
                foreach (var message in unreadMessages)
                    message.MessageIsRead = true;
                await _context.SaveChangesAsync();
                return Ok(new { message = $"{unreadMessages.Count} mesaj okundu olarak işaretlendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    title = "Mesajlar güncellenemedi",
                    detail = ex.Message
                });
            }
        }
    }
}