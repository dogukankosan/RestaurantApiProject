namespace RestaurantAPI.Dtos.IconDtos
{
    public class CreateIconDto
    {
        public string IconURL { get; set; }
        public bool IconStatus { get; set; } = true;
    }
}