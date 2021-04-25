using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Webshop.Domain.DTOs;
using Webshop.Domain.DTOs.Cart;
using Webshop.Domain.Models;
using Webshop.Repositories.AddressRepository;
using Webshop.Repositories.CartRepository;
using Webshop.Repositories.MembershipRepository;
using Webshop.Repositories.PaymentRepository;
using Webshop.Repositories.UserRepository;
using Webshop.Services.DatetimeService;
using Webshop.Utils.Extensions;

namespace Webshop.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository,
            ICartRepository cartRepository,
            IMembershipRepository membershipRepository,
            IMapper mapper,
            IDateTimeService dateTimeService,
            IAddressRepository addressRepository,
            IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _membershipRepository = membershipRepository;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _addressRepository = addressRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<UserDto> CreateGuestUserAsync()
        {
            var cart = new Cart();
            cart = await _cartRepository.CreateAsync(cart);
            var membership = await _membershipRepository.GetAsync("Guest");
            membership.ThrowIfNull("Guest");

            var user = await _userRepository.CreateAsync(new User
            {
                CartId = cart.Id,
                MembershipId = membership.Id
            });

            return _mapper.Map<UserDto>(user);
        }

        public Task UpdateUser(UserDto user)
        {
            return _userRepository.UpdateAsync(_mapper.Map<User>(user));
        }

        public async Task UpdateMembershipAsync(int id, int membershipId)
        {
            var user = await _userRepository.GetAsync(id);
            user.ThrowIfNull(id);

            var membership = await _membershipRepository.GetAsync(membershipId);
            membership.ThrowIfNull(membershipId);

            user.MembershipId = membershipId;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<CartOverview> GetCartOverviewAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            user.ThrowIfNull(id);

            if (!user.Cart.CartProducts.Any()) return new CartOverview();

            var cartOverview = new CartOverview
            {
                User = _mapper.Map<UserDto>(user),
                CartId = user.CartId,
                Products = user.Cart.CartProducts.Select(ToProductOverview).ToList()
            };

            cartOverview.TotalPrice = cartOverview.Products.Sum(_ => _.TotalPrice);
            cartOverview.TotalDiscountedPrice = cartOverview.Products.Sum(_ => _.TotalDiscountedPrice);
            cartOverview.FinalPrice = cartOverview.Products.Sum(_ => _.FinalPrice);

            return cartOverview;
        }

        public async Task<AddressDto> GetAddressAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            user.ThrowIfNull(id);

            return user.Address is null ? new AddressDto() : _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<PaymentDto> GetPaymentAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            user.ThrowIfNull(id);

            return user.Payment is null ? new PaymentDto() : _mapper.Map<PaymentDto>(user.Payment);
        }

        public async Task<AddressDto> CreateAddressAsync(int id, CreateAddress createAddress)
        {
            var user = await _userRepository.GetAsync(id);
            user.ThrowIfNull(id);
            var address = await _addressRepository.CreateAsync(_mapper.Map<Address>(createAddress));

            user.AddressId = address.Id;
            await _userRepository.UpdateAsync(user);
            return _mapper.Map<AddressDto>(address);
        }

        public async Task<PaymentDto> CreatePaymentAsync(int id, CreatePayment createPayment)
        {
            var user = await _userRepository.GetAsync(id);
            user.ThrowIfNull(id);
            var payment = await _paymentRepository.CreateAsync(_mapper.Map<Payment>(createPayment));

            user.PaymentId = payment.Id;
            await _userRepository.UpdateAsync(user);
            return _mapper.Map<PaymentDto>(payment);
        }

        private CartProductOverview ToProductOverview(CartProduct cartProduct)
        {
            var cartProductOverview = _mapper.Map<CartProductOverview>(cartProduct.Product);

            cartProductOverview.TotalQuantity = cartProduct.Quantity;
            cartProductOverview.SetTotalPrice();

            var discount = cartProduct.Product.Discount;

            if (!discount.IsAvailable(_dateTimeService.GetCurrentUtc()))
            {
                cartProductOverview.SetFinalPrice();
                return cartProductOverview;
            }

            ApplyProductDiscountCalculations(cartProductOverview, discount);
            return cartProductOverview;
        }

        private static void ApplyProductDiscountCalculations(CartProductOverview cartProductOverview, Discount discount)
        {
            cartProductOverview.TotalDiscountedQuantity =
                cartProductOverview.TotalQuantity.GetMaxQuantityToDiscount(discount.MaxQuantity, discount.MinQuantity);
            cartProductOverview.SetNonDiscountedTotals();

            cartProductOverview.UnitDiscountedPrice =
                cartProductOverview.TotalDiscountedQuantity.ApplyDiscount(cartProductOverview.UnitPrice,
                    discount.Percentage);
            cartProductOverview.SetTotalDiscountedPrice();
            cartProductOverview.SetFinalPrice();
        }
    }
}