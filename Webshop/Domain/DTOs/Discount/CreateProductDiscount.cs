namespace Webshop.Domain.DTOs.Discount
{
    public class CreateProductDiscount
    {
        public int ProductId { get; set; }
        public CreateDiscount Discount { get; set; }
    }
}