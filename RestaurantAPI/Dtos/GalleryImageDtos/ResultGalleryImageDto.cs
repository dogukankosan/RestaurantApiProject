namespace RestaurantAPI.Dtos.GalleryImageDtos
{
    public class ResultGalleryImageDto
    {
        public int ImageID { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageByte { get; set; }
        public bool ImageStatus { get; set; }
    }
}