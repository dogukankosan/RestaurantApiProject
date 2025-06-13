using System.ComponentModel;
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
        [Column(TypeName = "nvarchar(100)")]
        public string CategoryName { get; set; }

        [DefaultValue(true)]
        public bool CategoryStatus { get; set; } = true;

        public List<Product> Products { get; set; }
    }
}