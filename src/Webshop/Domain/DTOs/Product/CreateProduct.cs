namespace Webshop.Domain.DTOs.Product
{
    public class CreateProduct
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PackageQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string PackageType { get; set; }
    }
}