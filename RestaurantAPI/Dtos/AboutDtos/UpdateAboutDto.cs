namespace RestaurantAPI.Dtos.AboutDtos
{
    public class UpdateAboutDto
    {
        public int AboutID { get; set; }
        public byte[] AboutCompanyLogo { get; set; }
        public string AboutCompanyName { get; set; }
        public byte[] AboutImage1 { get; set; }
        public byte[] AboutImage2 { get; set; }
        public string AboutDesc { get; set; }
        public string AboutWhyChoose { get; set; }
        public byte[] AboutReportImage { get; set; }
        public byte[] AboutRezervationImage { get; set; }
    }
}