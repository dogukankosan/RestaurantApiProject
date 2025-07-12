namespace RestaurantWebUI.Dtos.AboutDtos
{
    public class UpdateAboutDto
    {
        public int AboutID { get; set; }
        public string AboutCompanyName { get; set; }
        public string AboutDesc { get; set; } 
        public string AboutWhyChoose { get; set; } 
        public byte[]? AboutCompanyLogo { get; set; }
        public byte[]? AboutImage1 { get; set; }
        public byte[]? AboutImage2 { get; set; }
        public byte[]? AboutReportImage { get; set; }
        public byte[]? AboutRezervationImage { get; set; }
        public IFormFile? AboutCompanyLogoFile { get; set; }
        public IFormFile? AboutImage1File { get; set; }
        public IFormFile? AboutImage2File { get; set; }
        public IFormFile? AboutReportImageFile { get; set; }
        public IFormFile? AboutRezervationImageFile { get; set; }
        public string? AboutCompanyLogoBase64 { get; set; }
        public string? AboutImage1Base64 { get; set; }
        public string? AboutImage2Base64 { get; set; }
        public string? AboutReportImageBase64 { get; set; }
        public string? AboutRezervationImageBase64 { get; set; }

    }
}