namespace RestaurantWebUI.Dtos.TestimonialDtos
{
    public class GetByIDTestimonialDto
    {
        public int TestimonialID { get; set; }
        public string TestimonialNameSurname { get; set; }
        public string TestimonialTitle { get; set; }
        public string TestimonialComment { get; set; }
        public byte[] TestimonialImage { get; set; }
        public bool TestimonialStatus { get; set; }
    }
}