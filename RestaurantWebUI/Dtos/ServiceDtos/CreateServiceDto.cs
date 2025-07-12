using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestaurantWebUI.Dtos.ServiceDtos
{
    public class CreateServiceDto
    {
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceIconUrl { get; set; }
        public bool ServiceStatus { get; set; }
        public List<SelectListItem>? IconOptions { get; set; } 
    }
}