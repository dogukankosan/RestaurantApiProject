namespace RestaurantAPI.Dtos.FeatureDtos
{
    public class GetByIDFeatureDto
    {
        public int FeatureID { get; set; }
        public string FeatureTitle { get; set; }
        public string FeatureSubTitle { get; set; }
        public string FeatureDescription { get; set; }
        public string FeatureVideoUrl { get; set; }
        public byte[] FeatureImageData { get; set; }
    }
}