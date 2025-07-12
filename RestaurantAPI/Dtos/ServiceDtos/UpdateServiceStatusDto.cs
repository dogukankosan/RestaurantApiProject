namespace RestaurantAPI.Dtos.ServiceDtos
{
    public class UpdateServiceStatusDto
    {
        public int ServiceID { get; set; }
        public bool IsActive { get; set; }
    }
}