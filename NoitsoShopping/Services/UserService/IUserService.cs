using NoitsoShopping.Domain.DTOs;
using System.Threading.Tasks;

namespace NoitsoShopping.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> CreateGuestUserAsync();
    }
}