namespace RestaurantAPI.Dtos.ServiceDtos
{
    public class CreateServiceDto
    {
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceIconUrl { get; set; }
        public bool ServiceStatus { get; set; }
    }
}