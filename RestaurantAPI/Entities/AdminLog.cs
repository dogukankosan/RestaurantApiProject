using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class AdminLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string LogType { get; set; }
        public string LogMessage { get; set; }
        [Column(TypeName = "nvarchar(25)")]
        public string LogUserIPAdress { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string LogUserInfo { get; set; }
        [MaxLength(75)]
        public string LogUserGeo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}