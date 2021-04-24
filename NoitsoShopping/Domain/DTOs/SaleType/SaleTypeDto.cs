namespace NoitsoShopping.Domain.DTOs.SaleType
{
    public class SaleTypeDto
    {
        public int Id { get; set; }
        public Utils.Enums.SaleType SaleType { get; set; }
        public BaseSaleConfiguration Configuration { get; set; }
    }
}