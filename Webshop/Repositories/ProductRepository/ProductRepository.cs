using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Models;

namespace Webshop.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebshopContext _dbContext;

        public ProductRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Product>> GetAsync(IReadOnlyCollection<int> ids)
        {
            var product = await _dbContext
                .Products
                .Where(_ => ids.Contains(_.Id))
                .ToListAsync();
            return product;
        }

        public async Task<Product> GetAsync(int id)
        {
            var product = await _dbContext
                .Products
                .Include(_ => _.Discount)
                .FirstOrDefaultAsync(_ => _.Id == id);
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

        public Task UpdateAsync(Product product)
        {
            _dbContext.Update(product);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(ICollection<Product> products)
        {
            _dbContext.UpdateRange(products);
            return _dbContext.SaveChangesAsync();
        }
    }
}