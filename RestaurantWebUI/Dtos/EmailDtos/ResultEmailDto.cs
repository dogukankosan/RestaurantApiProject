namespace RestaurantWebUI.Dtos.EmailDtos
{
    public class ResultEmailDto
    {
        public int EmailID { get; set; }
        public string EmailBox { get; set; }
        public string EmailPassword { get; set; }
        public string EmailServer { get; set; }
        public int EmailSSl { get; set; }
        public int EmailPort { get; set; }
        public string EmailCompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPhone { get; set; }
        public byte[] EmailImage { get; set; }
    }
}