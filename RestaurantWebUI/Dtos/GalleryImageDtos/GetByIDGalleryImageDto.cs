namespace RestaurantWebUI.Dtos.GalleryImageDtos
{
    public class GetByIDGalleryImageDto
    {
        public int ImageID { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageByte { get; set; }
        public bool ImageStatus { get; set; }
    }
}