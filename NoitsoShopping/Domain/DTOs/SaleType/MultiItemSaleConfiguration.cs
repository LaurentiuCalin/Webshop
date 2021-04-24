namespace NoitsoShopping.Domain.DTOs.SaleType
{
    public class MultiItemSaleConfiguration : BaseSaleConfiguration
    {
        public int TotalNumberOfItems { get; set; }
        public int DiscountedNumberOfItems { get; set; }
    }
}