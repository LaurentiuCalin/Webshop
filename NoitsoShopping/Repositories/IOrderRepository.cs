using System.Collections.Generic;
using System.Threading.Tasks;
using NoitsoShopping.Domain.DTOs;
using NoitsoShopping.Domain.DTOs.Order;

namespace NoitsoShopping.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderCreatedDto> CreateAsync(int customerId, AddressDto address, PaymentDto payment, List<ProductDto> products);
    }
}