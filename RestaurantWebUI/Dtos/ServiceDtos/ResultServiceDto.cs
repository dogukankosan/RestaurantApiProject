namespace RestaurantWebUI.Dtos.ServiceDtos
{
    public class ResultServiceDto
    {
        public int ServiceID { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceIconUrl { get; set; }
        public bool ServiceStatus { get; set; }
    }
}