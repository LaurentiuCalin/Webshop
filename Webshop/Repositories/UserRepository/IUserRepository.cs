using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<User> GetAsync(int id);
    }
}