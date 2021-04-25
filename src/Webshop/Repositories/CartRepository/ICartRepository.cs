using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.CartRepository
{
    public interface ICartRepository
    {
        Task<Cart> CreateAsync(Cart cart);
        Task<Cart> GetAsync(int id);
        Task DeleteProductsAsync(ICollection<CartProduct> cartProducts);
        Task AddProductAsync(CartProduct cartProduct);
        Task UpdateProductAsync(CartProduct cartProduct);
    }
}