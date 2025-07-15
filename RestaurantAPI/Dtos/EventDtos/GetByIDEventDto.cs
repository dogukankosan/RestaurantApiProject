namespace RestaurantAPI.Dtos.EventDtos
{
    public class GetByIDEventDto
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public decimal EventPrice { get; set; }
        public string EventPriceSembol { get; set; }
        public string EventDetails { get; set; }
        public byte[] EventImage { get; set; }
        public bool EventStatus { get; set; }
    }
}