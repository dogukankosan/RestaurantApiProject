namespace RestaurantAPI.Dtos.AdminLogDtos
{
    public class ResultAdminLogDto
    {
        public int LogID { get; set; }
        public string LogType { get; set; }
        public string LogMessage { get; set; }
        public string LogUserIPAdress { get; set; }
        public string LogUserInfo { get; set; }
        public string LogUserGeo { get; set; }
        public DateTime LogDate { get; set; }
    }
}