using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class GalleryImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageID { get; set; }
        [MaxLength(100)]
        public string ImageTitle { get; set; }
        [Required]
        public byte[] ImageByte { get; set; }

        [DefaultValue(true)]
        public bool ImageStatus { get; set; } = true;
    }
}