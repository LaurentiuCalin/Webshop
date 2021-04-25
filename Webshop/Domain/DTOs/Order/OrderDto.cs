using System.Collections.Generic;

namespace Webshop.Domain.DTOs.Order
{
    public class OrderDto
    {
        public UserDto User { get; set; }
        public AddressDto Address { get; set; }
        public PaymentDto Payment { get; set; }
        public ICollection<OrderProductDto> OrderProducts { get; set; }
    }
}