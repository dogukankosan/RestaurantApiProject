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
        public byte[] ChefImage { get; set; }
        public bool ChefStatus { get; set; } = true;
    }
}