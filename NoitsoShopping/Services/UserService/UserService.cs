using System.Threading.Tasks;
using AutoMapper;
using NoitsoShopping.Domain.DTOs;
using NoitsoShopping.Domain.Models;
using NoitsoShopping.Repositories.CartRepository;
using NoitsoShopping.Repositories.MembershipRepository;
using NoitsoShopping.Repositories.UserRepository;

namespace NoitsoShopping.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            ICartRepository cartRepository,
            IMembershipRepository membershipRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _membershipRepository = membershipRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateGuestUserAsync()
        {
            var cart = await _cartRepository.CreateAsync();
            var membership = await _membershipRepository.GetMembershipAsync("Guest");
            var user = await _userRepository.CreateAsync(new User
            {
                CartId = cart.Id,
                MembershipId = membership.Id
            });

            return _mapper.Map<UserDto>(user);
        }


    }
}