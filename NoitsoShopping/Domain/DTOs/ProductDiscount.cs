namespace NoitsoShopping.Domain.DTOs
{
    public class ProductDiscount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PackageQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string PackageType { get; set; }
        public string Category { get; set; }
        public DiscountDto Discount { get; set; }
    }
}