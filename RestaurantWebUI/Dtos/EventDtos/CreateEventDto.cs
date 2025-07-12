using System.ComponentModel.DataAnnotations;

namespace RestaurantWebUI.Dtos.EventDtos
{
    public class CreateEventDto
    {
        public string EventName { get; set; }
        public decimal EventPrice { get; set; }
        public string EventDetails { get; set; }
        public byte[] EventImage { get; set; }
        public IFormFile? EventImageFile { get; set; }
        public string? EventImageBase64 { get; set; }
        public bool EventStatus { get; set; } 
    }
}