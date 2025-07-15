using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Email
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailID { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? EmailBox { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? EmailPassword { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? EmailServer { get; set; }

        [Required]
        public int EmailSSl { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int EmailPort { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? EmailCompanyName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? EmailAddress { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string? EmailPhone { get; set; }

        public byte[]? EmailImage { get; set; }
    }
}