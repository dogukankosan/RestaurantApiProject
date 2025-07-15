using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string ReservationNameSurname { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [EmailAddress]
        public string ReservationEmail { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(25)")]
        public string ReservationPhone { get; set; }
        [Column(TypeName = "nvarchar(25)")]
        public string WebLogUserIPAdress { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string WebLogUserInfo { get; set; }
        [Column(TypeName = "nvarchar(75)")]
        public string WebLogUserGeo { get; set; }
        [Column(TypeName = "smalldatetime")]
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        [Range(1, 999999)]
        public int ReservationCountOfPeople { get; set; }
        public string ReservationMessage { get; set; }
        [DefaultValue(0)]
        public int ReservationStatus { get; set; } = 0;
        [DefaultValue(false)]
        public bool ReservationIsRead { get; set; } = false;
        [Column(TypeName = "smalldatetime")]
        [Required]
        public DateTime ReservationCreateDate { get; set; }
    }
}