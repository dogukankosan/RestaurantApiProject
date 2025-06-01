using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; } 
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
        public bool CategoryStatus { get; set; } = true;
        public List<Product> Products { get; set; }
    }
}