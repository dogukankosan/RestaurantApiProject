using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class WebLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WebLogID { get; set; }
        [MaxLength(25)]
        public string WebLogUserIPAdress { get; set; }
        [MaxLength(250)]
        public string WebLogUserInfo { get; set; }
        [MaxLength(75)]
        public string WebLogUserGeo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime WebLogDate { get; set; } = DateTime.Now;
    }
}