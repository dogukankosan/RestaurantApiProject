using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Icon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IconID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string IconURL { get; set; }
        public bool IconStatus { get; set; } = true;
    }
}