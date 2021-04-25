using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebshopContext _dbContext;

        public OrderRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _dbContext.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
    }
}