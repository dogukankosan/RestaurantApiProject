namespace RestaurantAPI.Dtos.IconDtos
{
    public class GetByIDIconDto
    {
        public int IconID { get; set; }
        public string IconURL { get; set; }
        public bool IconStatus { get; set; } = true;
    }
}