namespace RestaurantWebUI.Dtos.FeatureDtos
{
    public class UpdateFeatureDto
    {
        public int FeatureID { get; set; }
        public string FeatureTitle { get; set; }
        public string FeatureSubTitle { get; set; }
        public string FeatureDescription { get; set; }
        public string FeatureVideoUrl { get; set; }
        public IFormFile? FeatureImageFile { get; set; }
        public byte[]? FeatureImageData { get; set; }
        public string? FeatureImageBase64 { get; set; }
    }
}