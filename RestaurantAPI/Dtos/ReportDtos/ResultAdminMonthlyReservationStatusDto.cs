namespace RestaurantAPI.Dtos.ReportDtos
{
    public class ResultAdminMonthlyReservationStatusDto
    {
        public string Month { get; set; } 
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Pending { get; set; }
    }
}