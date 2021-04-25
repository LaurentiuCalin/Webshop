using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly WebshopContext _dbContext;

        public CartRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cart> CreateAsync()
        {
            var cart = new Cart();
            _dbContext.Add(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> GetAsync(int id)
        {
            var cart = await _dbContext.Carts
                .Include(_ => _.CartProducts)
                .ThenInclude(_ => _.Product)
                .SingleOrDefaultAsync(_ => _.Id == id);

            return cart;
        }

        public Task AddProductAsync(CartProduct cartProduct)
        {
            _dbContext.Add(cartProduct);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateProductAsync(CartProduct cartProduct)
        {
            _dbContext.Update(cartProduct);
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteProductsAsync(ICollection<CartProduct> cartProducts)
        {
            _dbContext.RemoveRange(cartProducts);
            return _dbContext.SaveChangesAsync();
        }
    }
}