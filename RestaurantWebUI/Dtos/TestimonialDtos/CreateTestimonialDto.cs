namespace RestaurantWebUI.Dtos.TestimonialDtos
{
    public class CreateTestimonialDto
    {
        public string TestimonialNameSurname { get; set; }
        public string TestimonialTitle { get; set; }
        public string TestimonialComment { get; set; }
        public IFormFile? TestimonialImageFile { get; set; }
        public string? TestimonialImageBase64 { get; set; }
        public byte[]? TestimonialImage { get; set; }
        public bool TestimonialStatus { get; set; }
    }
}