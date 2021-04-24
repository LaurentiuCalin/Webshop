using System.Threading.Tasks;
using NoitsoShopping.Domain.Models;

namespace NoitsoShopping.Repositories.CartRepository
{
    public interface ICartRepository
    {
        Task<Cart> CreateAsync();
        Task<Cart> GetAsync(int id);
        Task AddProductAsync(CartProduct cartProduct);
        Task UpdateProductAsync(CartProduct cartProduct);
    }
}