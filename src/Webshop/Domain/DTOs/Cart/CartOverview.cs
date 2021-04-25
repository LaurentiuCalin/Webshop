using System.Collections.Generic;

namespace Webshop.Domain.DTOs.Cart
{
    public class CartOverview
    {
        public UserDto User { get; set; }
        public int CartId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public ICollection<CartProductOverview> Products { get; set; }
    }
}