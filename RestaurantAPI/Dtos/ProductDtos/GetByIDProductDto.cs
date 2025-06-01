namespace RestaurantAPI.Dtos.ProductDtos
{
    public class GetByIDProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public byte[] ProductImageData { get; set; }
        public bool ProductStatus { get; set; } = true;
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}