using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactID { get; set; } 
        [Required]
        public string ContactMapLocation { get; set; }
        [Required]
        [MaxLength(500)]
        public string ContactAddress { get; set; }
        [Required]
        [MaxLength(20)]
        public string ContactPhone { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Required]
        [MaxLength(50)]
        public string ContactOpenHours { get; set; }
    }
}