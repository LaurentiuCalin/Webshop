using NoitsoShopping.Domain.Models;
using System.Threading.Tasks;

namespace NoitsoShopping.Repositories.UserRepository
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
    }
}