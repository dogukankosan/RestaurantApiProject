namespace RestaurantAPI.Dtos.ChefDtos
{
    public class UpdateChefStatusDto
    {
        public int ChefID { get; set; }
        public bool IsActive { get; set; }
    }
}