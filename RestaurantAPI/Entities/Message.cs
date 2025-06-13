using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string MessageNameSurname { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [EmailAddress]
        public string MessageEmail { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string MessagePhone { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(300)")]
        public string MessageSubject { get; set; }
        [Required]
        public string MessageDetails { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime MessageSendDate { get; set; }= DateTime.Now;
        [DefaultValue(false)]
        public bool MessageIsRead { get; set; }=false;
    }
}