using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Testimonial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestimonialID { get; set; }
        [Required]
        [MaxLength(100)]
        public string TestimonialNameSurname { get; set; }
        [MaxLength(100)]
        public string TestimonialTitle { get; set; }
        [Required]
        [MaxLength(1000)]
        public string TestimonialComment { get; set; }
        [Required]
        public byte[] TestimonialImage { get; set; }

        [DefaultValue(true)] 
        public bool TestimonialStatus { get; set; } = true;
    }
}