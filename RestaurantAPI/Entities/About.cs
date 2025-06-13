using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class About
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AboutID { get; set; }
        [Required]
        public byte[] AboutCompanyLogo { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string AboutCompanyName { get; set; }
        [Required]
        public byte[] AboutImage1 { get; set; }
        [Required]
        public byte[] AboutImage2 { get; set; }
        [Required]
        public string AboutDesc { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string AboutWhyChoose { get; set; }
        [Required]
        public byte[] AboutReportImage { get; set; }
        [Required]
        public byte[] AboutRezervationImage { get; set; }
    }
}