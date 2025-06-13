using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string EventName { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 999999)]
        public decimal EventPrice { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string EventDetails { get; set; }
        [Required]
        [Column(TypeName = "varbinary(max)")]
        public byte[] EventImage { get; set; }
        [DefaultValue(true)]
        public bool EventStatus { get; set; } = true;
    }
}