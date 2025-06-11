using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Chef
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChefID { get; set; }
        [Required]
        [MaxLength(100)]
        public string ChefNameSurname { get; set; }
        [Required]
        [MaxLength(200)]
        public string ChefTitle { get; set; }
        public string ChefDescription { get; set; }
        [MaxLength(100)]
        public string ChefTwitterLink { get; set; }
        [MaxLength(100)]
        public string ChefFacebookLink { get; set; }
        [MaxLength(100)]
        public string ChefInstagramLink { get; set; }
        [MaxLength(100)]
        public string ChefLinkedinLink { get; set; }
        [Required]
        public byte[] ChefImage { get; set; }
        [DefaultValue(true)]
        public bool ChefStatus { get; set; } = true;
    }
}