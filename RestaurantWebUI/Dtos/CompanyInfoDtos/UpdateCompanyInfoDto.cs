namespace RestaurantWebUI.Dtos.CompanyInfoDtos
{
    public class UpdateCompanyInfoDto
    {
        public int CompanyInfoID { get; set; }
        public string CompanyInfoAddress { get; set; }
        public string CompanyInfoPhone { get; set; }
        public string CompanyInfoMail { get; set; }
        public string CompanyInfoOpenClosed { get; set; }
        public string? CompanyInfoGithubLink { get; set; }
        public string? CompanyInfoWebSiteLink { get; set; }
        public string? CompanyInfoInstagramLink { get; set; }
        public string? CompanyInfoLinkedinLink { get; set; }
        public string CompanyInfoIFrame { get; set; }
    }
}