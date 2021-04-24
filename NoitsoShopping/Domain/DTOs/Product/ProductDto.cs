namespace NoitsoShopping.Domain.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PackageQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string PackageType { get; set; }
        public CategoryDto Category { get; set; }
        public DiscountDto Discount { get; set; }
    }
}