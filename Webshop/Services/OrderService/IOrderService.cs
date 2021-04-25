using System.Threading.Tasks;
using Webshop.Domain.DTOs;
using Webshop.Domain.DTOs.Cart;
using Webshop.Domain.DTOs.Order;

namespace Webshop.Services.OrderService
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CartOverview cartOverview, AddressDto address, PaymentDto payment);
    }
}