namespace Webshop.Domain.DTOs.Order
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}