namespace RestaurantWebUI.Dtos.ReservationDtos
{
    public class GetByIDReservationDto
    {
        public int ReservationID { get; set; }
        public string ReservationNameSurname { get; set; }
        public string ReservationEmail { get; set; }
        public string ReservationPhone { get; set; }
        public DateTime ReservationDate { get; set; }
        public int ReservationCountOfPeople { get; set; }
        public string ReservationMessage { get; set; }
        public string WebLogUserIPAdress { get; set; }
        public string WebLogUserInfo { get; set; }
        public string WebLogUserGeo { get; set; }
        public int ReservationStatus { get; set; }
        public bool ReservationIsRead { get; set; }
        public DateTime ReservationCreateDate { get; set; }
    }
}