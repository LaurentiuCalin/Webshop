using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(int id);
        Task<ICollection<Product>> GetAsync(IReadOnlyCollection<int> ids);
        Task<List<Product>> GetAsync();
        Task UpdateAsync(Product product);
        Task UpdateAsync(ICollection<Product> products);
    }
}