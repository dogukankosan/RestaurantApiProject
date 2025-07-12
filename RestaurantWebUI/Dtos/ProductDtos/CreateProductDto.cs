using RestaurantWebUI.Dtos.CategoryDtos;

namespace RestaurantWebUI.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public char ProductPriceSembol { get; set; }
        public IFormFile ProductImageFile { get; set; }
        public byte[] ProductImageData { get; set; }
        public string? ProductImageBase64 { get; set; } 
        public bool ProductStatus { get; set; } = true;
        public int CategoryID { get; set; }
        public List<ResultCategoryDto> CategoryList { get; set; } = new();
    }
}