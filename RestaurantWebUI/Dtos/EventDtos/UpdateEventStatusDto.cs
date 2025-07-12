namespace RestaurantWebUI.Dtos.EventDtos
{
    public class UpdateEventStatusDto
    {
        public int EventID { get; set; }
        public bool IsActive { get; set; }
    }
}