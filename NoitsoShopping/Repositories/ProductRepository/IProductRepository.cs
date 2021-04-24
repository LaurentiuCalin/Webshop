using NoitsoShopping.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoitsoShopping.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(int id);
        Task<List<Product>> GetAsync();
    }
}