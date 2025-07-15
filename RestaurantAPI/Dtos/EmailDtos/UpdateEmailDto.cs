using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Dtos.EmailDtos
{
    public class UpdateEmailDto
    {
        public int EmailID { get; set; }
        public string? EmailBox { get; set; }
        public string? EmailPassword { get; set; }
        public string? EmailServer { get; set; }
        public int EmailSSl { get; set; }
        public int EmailPort { get; set; }
        public string? EmailCompanyName { get; set; }
        public string? EmailAddress { get; set; }
        public string? EmailPhone { get; set; }
        public byte[]? EmailImage { get; set; }
    }
}