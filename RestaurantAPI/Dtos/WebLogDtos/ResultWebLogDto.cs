namespace RestaurantAPI.Dtos.WebLogDtos
{
    public class ResultWebLogDto
    {
        public int WebLogID { get; set; }
        public string WebLogUserIPAdress { get; set; }
        public string WebLogUserInfo { get; set; }
        public string WebLogUserGeo { get; set; }
        public DateTime WebLogDate { get; set; }
    }
}