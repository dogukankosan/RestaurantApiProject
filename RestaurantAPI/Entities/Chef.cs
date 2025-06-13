using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Chef
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChefID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string ChefNameSurname { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string ChefTitle { get; set; }
        public string ChefDescription { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ChefTwitterLink { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ChefFacebookLink { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ChefInstagramLink { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ChefLinkedinLink { get; set; }
        [Required]
        public byte[] ChefImage { get; set; }
        [DefaultValue(true)]
        public bool ChefStatus { get; set; } = true;
    }
}