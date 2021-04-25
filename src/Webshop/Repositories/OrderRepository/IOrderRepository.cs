using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}