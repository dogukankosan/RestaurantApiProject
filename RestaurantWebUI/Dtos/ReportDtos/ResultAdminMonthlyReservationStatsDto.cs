namespace RestaurantWebUI.Dtos.ReportDtos
{
    public class ResultAdminMonthlyReservationStatsDto
    {
        public string Month { get; set; } 
        public int Arrived { get; set; }
        public int NotArrived { get; set; }
    }
}