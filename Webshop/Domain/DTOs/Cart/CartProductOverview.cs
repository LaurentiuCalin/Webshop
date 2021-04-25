namespace Webshop.Domain.DTOs.Cart
{
    public class CartProductOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int PackageQuantity { get; set; }
        public string PackageType { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal UnitDiscountedPrice { get; set; }

        public int TotalQuantity { get; set; }
        public int TotalDiscountedQuantity { get; set; }
        public int TotalNonDiscountedQuantity { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
        public decimal TotalNonDiscountedPrice { get; set; }

        public decimal FinalPrice { get; set; }

        public void SetTotalPrice()
        {
            TotalPrice = TotalQuantity * UnitPrice;
        }

        public void SetTotalDiscountedPrice()
        {
            TotalDiscountedPrice = TotalDiscountedQuantity * UnitDiscountedPrice;
        }

        public void SetNonDiscountedTotals()
        {
            TotalNonDiscountedQuantity = TotalQuantity - TotalDiscountedQuantity;
            TotalNonDiscountedPrice = TotalNonDiscountedQuantity * UnitPrice;
        }

        public void SetFinalPrice()
        {
            FinalPrice = TotalPrice - UnitDiscountedPrice;
        }
    }
}