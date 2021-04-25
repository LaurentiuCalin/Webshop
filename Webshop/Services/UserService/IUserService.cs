using System.Threading.Tasks;
using Webshop.Domain.DTOs;
using Webshop.Domain.DTOs.Cart;

namespace Webshop.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> CreateGuestUserAsync();
        Task UpdateUser(UserDto user);
        Task UpdateMembershipAsync(int id, int membershipId);

        Task<CartOverview> GetCartOverviewAsync(int id);

        Task<AddressDto> GetAddressAsync(int id);
        Task<PaymentDto> GetPaymentAsync(int id);

        Task<AddressDto> CreateAddressAsync(int id, CreateAddress createAddress);
        Task<PaymentDto> CreatePaymentAsync(int id, CreatePayment payment);
    }
}