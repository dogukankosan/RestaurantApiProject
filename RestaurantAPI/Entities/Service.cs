using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceID { get; set; } 
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string ServiceTitle { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string ServiceDescription { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string ServiceIconUrl { get; set; }

        [DefaultValue(true)] 
        public bool ServiceStatus { get; set; } = true;
    }
}