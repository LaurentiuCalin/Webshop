using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Models;

namespace Webshop.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly WebshopContext _dbContext;

        public UserRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public Task UpdateAsync(User user)
        {
            _dbContext.Update(user);
            return _dbContext.SaveChangesAsync();
        }

        public Task<User> GetAsync(int id)
        {
            return _dbContext.Users
                .Include(_ => _.Address)
                .Include(_ => _.Payment)
                .Include(_ => _.Cart)
                .ThenInclude(_ => _.CartProducts)
                .ThenInclude<User, CartProduct, Product>(_ => _.Product)
                .ThenInclude(_ => _.Discount)
                .SingleOrDefaultAsync(_ => _.Id == id);
        }
    }
}