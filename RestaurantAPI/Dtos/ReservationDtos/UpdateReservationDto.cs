namespace RestaurantAPI.Dtos.ReservationDtos
{
    public class UpdateReservationDto:CreateReservationDto
    {
        public int ReservationID { get; set; }
    }
}