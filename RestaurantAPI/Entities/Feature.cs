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
        [MaxLength(150)]
        public string FeatureTitle { get; set; }
        [Required]
        [MaxLength(150)]
        public string FeatureSubTitle { get; set; }
        [MaxLength(1000)]
        public string FeatureDescription { get; set; }
        [Required]
        [MaxLength(300)]
        public string FeatureVideoUrl { get; set; }
        [Required]
        public byte[] FeatureImageData { get; set; }
    }
}