namespace RestaurantAPI.Dtos.ReportDtos
{
    public class ResultAdminMonthlyReservationStatsDto
    {
        public int MonthNumber { get; set; }  
        public string Month { get; set; } 
        public int Arrived { get; set; }
        public int NotArrived { get; set; }
    }
}