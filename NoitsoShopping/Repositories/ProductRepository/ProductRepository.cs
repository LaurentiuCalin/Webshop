using Microsoft.EntityFrameworkCore;
using NoitsoShopping.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoitsoShopping.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebshopContext _dbContext;

        public ProductRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> GetAsync(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(_ => _.Id == id);
            return product;
        }

        public async Task<List<Product>> GetAsync()
        {
            var products = await _dbContext.Products
                .Include(_ => _.Discount)
                .Include(_ => _.Category)
                .ToListAsync();

            return products;
        }
    }
}