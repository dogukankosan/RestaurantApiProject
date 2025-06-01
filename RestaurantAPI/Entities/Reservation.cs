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
        [MaxLength(100)]
        public string ReservationNameSurname { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string ReservationEmail { get; set; }
        [Required]
        [MaxLength(20)]
        public string ReservationPhone { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        [Range(1, 999999)]
        public int ReservationCountOfPeople { get; set; }
        public string ReservationMessage { get; set; }
        public bool ReservationStatus { get; set; } = true;
    }
}