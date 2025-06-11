namespace RestaurantWebUI.Dtos.ReservationDtos
{
    public class ResultReservationDto
    {
        public int ReservationID { get; set; }
        public string ReservationNameSurname { get; set; }
        public string ReservationEmail { get; set; }
        public string ReservationPhone { get; set; }
        public DateTime ReservationDate { get; set; }
        public int ReservationCountOfPeople { get; set; }
        public string ReservationMessage { get; set; }
        public bool ReservationStatus { get; set; }
    }
}