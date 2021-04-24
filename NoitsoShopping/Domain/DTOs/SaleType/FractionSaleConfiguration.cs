namespace NoitsoShopping.Domain.DTOs.SaleType
{
    public class FractionSaleConfiguration : BaseSaleConfiguration
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
    }
}