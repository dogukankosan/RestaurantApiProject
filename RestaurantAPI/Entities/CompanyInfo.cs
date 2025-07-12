using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class CompanyInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyInfoID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string CompanyInfoAddress { get; set; } 
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string CompanyInfoPhone { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [EmailAddress]
        public string CompanyInfoMail { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string CompanyInfoOpenClosed { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CompanyInfoGithubLink { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CompanyInfoWebSiteLink { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CompanyInfoInstagramLink { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CompanyInfoLinkedinLink { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string CompanyInfoIFrame { get; set; }
    }
}