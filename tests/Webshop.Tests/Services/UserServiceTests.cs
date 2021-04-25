using System;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Core.Configurations;
using Webshop.Domain.DTOs;
using Webshop.Domain.DTOs.Cart;
using Webshop.Domain.Models;
using Webshop.Repositories.AddressRepository;
using Webshop.Repositories.CartRepository;
using Webshop.Repositories.MembershipRepository;
using Webshop.Repositories.PaymentRepository;
using Webshop.Repositories.UserRepository;
using Webshop.Services.DatetimeService;
using Webshop.Services.UserService;
using Xunit;

namespace Webshop.Tests.Unit.Services
{
    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture = new Fixture();
        private readonly IAddressRepository _addressRepository = Substitute.For<IAddressRepository>();
        private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
        private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();
        private readonly IMembershipRepository _membershipRepository = Substitute.For<IMembershipRepository>();
        private readonly IPaymentRepository _paymentRepository = Substitute.For<IPaymentRepository>();
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

        public UserServiceTests()
        {
            var mappingProfiles = new MappingProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfiles));
            _mapper = new Mapper(configuration);
            _sut = new UserService(_userRepository, _cartRepository, _membershipRepository, _mapper, _dateTimeService,
                _addressRepository, _paymentRepository);
        }

        [Fact]
        public async Task CreateGuestUserAsync_ShouldFail_WhenGuestMembershipDoesNotExist()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var expectedResult = new KeyNotFoundException($"{typeof(Membership)} with value Guest was not found");

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.CreateGuestUserAsync());

            // Assert
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task CreateGuestUserAsync_ShouldSucceed_WhenGuestMembershipExist()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var membership = new Membership { Id = 1, Label = "Guest" };
            var cart = _fixture.Create<Cart>();
            var user = new User { CartId = cart.Id, MembershipId = membership.Id };
            var userDto = _mapper.Map<UserDto>(user);

            _cartRepository.CreateAsync(new Cart()).ReturnsForAnyArgs(cart);
            _membershipRepository.GetAsync(membership.Label).ReturnsForAnyArgs(membership);
            _userRepository.CreateAsync(user).ReturnsForAnyArgs(user);

            // Act
            var result = await _sut.CreateGuestUserAsync();

            // Assert
            await _membershipRepository.Received(1).GetAsync("Guest");
            await _userRepository.Received(1).CreateAsync(Arg.Is<User>(_ => _.Id == user.Id));
            result.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task UpdateUser_ShouldSucceed()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var userDto = _fixture.Create<UserDto>();
            var user = _mapper.Map<User>(userDto);

            // Act
            await _sut.UpdateUser(userDto);

            // Assert
            await _userRepository.Received(1).UpdateAsync(Arg.Is<User>(_ => _.Id == user.Id));
        }

        [Fact]
        public async Task UpdateMembershipAsync_ShouldFail_WhenUserIsNull()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var membership = _fixture.Create<Membership>();
            var expectedResult = new KeyNotFoundException($"{typeof(User)} with id 1 was not found");
            _membershipRepository.GetAsync(membership.Id).Returns(membership);

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.UpdateMembershipAsync(1, membership.Id));

            // Assert
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task UpdateMembershipAsync_ShouldFail_WhenMembershipIsNull()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = _fixture.Create<User>();
            var expectedResult = new KeyNotFoundException($"{typeof(Membership)} with id 1 was not found");
            _userRepository.GetAsync(user.Id).Returns(user);

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.UpdateMembershipAsync(user.Id, 1));

            // Assert
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task UpdateMembershipAsync_ShouldSucceed_WhenUserAndMembershipExist()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = _fixture.Create<User>();
            var membership = _fixture.Create<Membership>();

            _userRepository.GetAsync(user.Id).Returns(user);
            _membershipRepository.GetAsync(membership.Id).Returns(membership);

            // Act
            await _sut.UpdateMembershipAsync(user.Id, membership.Id);

            // Assert
            await _userRepository.Received(1).UpdateAsync(Arg.Is<User>(_ => _.Id == user.Id && _.MembershipId == membership.Id));
        }

        [Fact]
        public async Task GetCartOverviewAsync_ShouldFail_WhenUserIsNull()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var expectedResult = new KeyNotFoundException($"{typeof(User)} with id 1 was not found");

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.GetCartOverviewAsync(1));

            // Assert
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task GetCartOverviewAsync_ShouldReturnEmptyCartOverview_WhenCartIsEmpty()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = _fixture.Create<User>();
            _userRepository.GetAsync(user.Id).Returns(user);
            user.Cart.CartProducts = new List<CartProduct>();

            // Act
            var result = await _sut.GetCartOverviewAsync(user.Id);

            // Assert
            await _userRepository.Received(1).GetAsync(user.Id);
            result.Should().BeEquivalentTo(new CartOverview());
        }

        [Fact]
        public async Task GetCartOverviewAsync_ShouldReturnCartOverviewWithoutDiscounts_WhenCartHasProductsWithoutDiscounts()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = _fixture.Create<User>();
            _userRepository.GetAsync(user.Id).Returns(user);

            foreach (var discount in user.Cart.CartProducts.Select(_=>_.Product.Discount))
            {
                discount.IsActive = false;
            }

            // Act
            var result = await _sut.GetCartOverviewAsync(user.Id);

            // Assert
            await _userRepository.Received(1).GetAsync(user.Id);
            result.Should().BeOfType(typeof(CartOverview));
        }

        [Fact]
        public async Task GetCartOverviewAsync_ShouldReturnCartOverviewWithDiscounts_WhenCartHasProductsWithDiscounts()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = _fixture.Create<User>();
            _userRepository.GetAsync(user.Id).Returns(user);
            var dateTime = _fixture.Create<DateTime>();

            foreach (var discount in user.Cart.CartProducts.Select(_=>_.Product.Discount))
            {
                discount.IsActive = true;
                discount.ValidFrom = null;
                discount.ValidUntil = null;
            }

            _dateTimeService.GetCurrentUtc().Returns(dateTime);

            // Act
            var result = await _sut.GetCartOverviewAsync(user.Id);

            // Assert
            await _userRepository.Received(1).GetAsync(user.Id);
            result.Should().BeOfType(typeof(CartOverview));
        }
    }
}