namespace RestaurantAPI.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductPriceSembol { get; set; }
        public byte[] ProductImageData { get; set; }
        public bool ProductStatus { get; set; } = true;
        public int CategoryID { get; set; }
    }
}