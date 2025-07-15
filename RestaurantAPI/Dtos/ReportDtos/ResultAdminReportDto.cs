namespace RestaurantAPI.Dtos.ReportDtos
{
    public class ResultAdminReportDto
    {
        public int PendingReservationCount { get; set; }
        public int TodayReservationCount { get; set; }
        public int TodayComingPeopleCount { get; set; }
        public string? NextCustomerName { get; set; }
        public int ApprovedReservationCount { get; set; }
        public int RejectedReservationCount { get; set; }
        public int ArrivedReservationCount { get; set; }
        public int NotArrivedReservationCount { get; set; }
        public int TodayMessageCount { get; set; }
        public int TotalMessageCount { get; set; }
        public int ProductCount { get; set; }
        public int ChefCount { get; set; }
        public int CategoryCount { get; set; }
        public int EventCount { get; set; }
        public int GalleryImageCount { get; set; }
        public int ServiceCount { get; set; }
        public int TestimonialCount { get; set; }
        public string? MostProductCategoryName { get; set; }
    }
}