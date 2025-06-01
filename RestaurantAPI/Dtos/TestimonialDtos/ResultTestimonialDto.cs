namespace RestaurantAPI.Dtos.TestimonialDtos
{
    public class ResultTestimonialDto
    {
        public int TestimonialID { get; set; }
        public string TestimonialNameSurname { get; set; }
        public string TestimonialTitle { get; set; }
        public string TestimonialComment { get; set; }
        public byte[] TestimonialImage { get; set; }
    }
}