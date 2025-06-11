namespace RestaurantWebUI.Dtos.GalleryImageDtos
{
    public class CreateGalleryImageDto
    {
        public string ImageTitle { get; set; }
        public byte[] ImageByte { get; set; }
        public bool ImageStatus { get; set; } 
    }
}