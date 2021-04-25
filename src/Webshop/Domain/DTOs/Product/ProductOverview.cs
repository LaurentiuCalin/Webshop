using Webshop.Domain.DTOs.Discount;

namespace Webshop.Domain.DTOs.Product
{
    public class ProductOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
        public int PackageQuantity { get; set; }
        public string PackageType { get; set; }
        public string Category { get; set; }
        public DiscountOverview DiscountOverview { get; set; }
    }
}