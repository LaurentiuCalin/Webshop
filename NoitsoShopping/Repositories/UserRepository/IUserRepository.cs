using System.Threading.Tasks;
using NoitsoShopping.Domain.Models;

namespace NoitsoShopping.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
    }
}