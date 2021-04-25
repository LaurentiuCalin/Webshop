namespace Webshop.Domain.Contracts
{
    public class ProductsFilter
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public PriceRange PriceRange { get; set; }
        public bool OnSale { get; set; }
        public bool InStock { get; set; }
    }

    public class PriceRange
    {
        public int UpperLimit { get; set; }
        public int LowerLimit { get; set; }
    }
}