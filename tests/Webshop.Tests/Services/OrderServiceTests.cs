using System.Linq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System.Threading.Tasks;
using NSubstitute.ExceptionExtensions;
using Webshop.Core.Configurations;
using Webshop.Domain.DTOs;
using Webshop.Domain.DTOs.Cart;
using Webshop.Domain.DTOs.Order;
using Webshop.Domain.DTOs.Product;
using Webshop.Domain.Models;
using Webshop.Repositories.OrderRepository;
using Webshop.Services.CartService;
using Webshop.Services.OrderService;
using Webshop.Services.ProductService;
using Webshop.Utils.Exceptions;
using Xunit;

namespace Webshop.Tests.Unit.Services
{
    public class OrderServiceTests
    {
        private readonly OrderService _sut;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture = new Fixture();
        private readonly ICartService _cartService = Substitute.For<ICartService>();
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private readonly IProductService _productService = Substitute.For<IProductService>();

        public OrderServiceTests()
        {
            var mappingProfiles = new MappingProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfiles));
            _mapper = new Mapper(configuration);
            _sut = new OrderService(_orderRepository, _mapper, _cartService, _productService);
        }

        [Fact]
        public async Task CreateAsync_ShouldSucceed_WhenAllPropertiesArePassed()
        {
            // Arrange
            var cartOverview = _fixture.Create<CartOverview>();
            var payment = _fixture.Create<PaymentDto>();
            var address = _fixture.Create<AddressDto>();

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var order = _fixture.Create<Order>();
            var expectedResult = _mapper.Map<OrderDto>(order);

            _orderRepository.CreateAsync(new Order()).ReturnsForAnyArgs(order);

            // Act
            var result = await _sut.CreateAsync(cartOverview, address, payment);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CreateAsync_ShouldFailAndDeleteOrder_WhenOrderedProductsExceedAvailability()
        {
            // Arrange
            var cartOverview = _fixture.Create<CartOverview>();
            var payment = _fixture.Create<PaymentDto>();
            var address = _fixture.Create<AddressDto>();
            var productAvailabilities = _fixture.CreateMany<SubtractProductAvailability>();

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var order = _fixture.Create<Order>();
            var product = order.OrderProducts.First().Product;
            var expectedResult =
                new ExceededProductQuantityException(product.Name, product.PackageType, product.AvailableQuantity, 1);

            _orderRepository.CreateAsync(new Order()).ReturnsForAnyArgs(order);
            _productService.SubtractAvailabilityAsync(productAvailabilities.ToList()).ThrowsForAnyArgs(expectedResult);

            // Act
            var result = await Assert.ThrowsAsync<ExceededProductQuantityException>(async () => await _sut.CreateAsync(cartOverview, address, payment));

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            await _orderRepository.Received(1).DeleteAsync(order);
        }
    }
}