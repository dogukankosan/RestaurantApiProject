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
        [MaxLength(200)]
        public string CompanyInfoAddress { get; set; } 
        [Required]
        [MaxLength(20)]
        public string CompanyInfoPhone { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string CompanyInfoMail { get; set; }
        [Required]
        [MaxLength(100)]
        public string CompanyInfoOpenClosed { get; set; }
        [Required]
        [MaxLength(100)]
        public string CompanyInfoGithubLink { get; set; }
        [Required]
        [MaxLength(100)]
        public string CompanyInfoWebSiteLink { get; set; }
        [Required]
        [MaxLength(100)]
        public string CompanyInfoInstagramLink { get; set; }
        [Required]
        [MaxLength(100)]
        public string CompanyInfoLinkedinLink { get; set; }
        [Required]
        [MaxLength(1000)]
        public string CompanyInfoIFrame { get; set; }
    }
}