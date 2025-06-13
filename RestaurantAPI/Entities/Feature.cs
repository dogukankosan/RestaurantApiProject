using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RestaurantAPI.Entities
{
    public class Feature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeatureID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string FeatureTitle { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string FeatureSubTitle { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string FeatureDescription { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(300)")]
        public string FeatureVideoUrl { get; set; }
        [Required]
        public byte[] FeatureImageData { get; set; }
    }
}