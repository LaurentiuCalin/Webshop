using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NSubstitute;
using Webshop.Domain.Models;
using Webshop.Repositories.CartRepository;
using Webshop.Repositories.ProductRepository;
using Webshop.Services.CartService;
using Webshop.Utils.Exceptions;
using Xunit;

namespace Webshop.Tests.Unit.Services
{
    public class CartServiceTests
    {
        private readonly CartService _sut;
        private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IFixture _fixture = new Fixture();

        public CartServiceTests()
        {
            _sut = new CartService(_cartRepository, _productRepository);
        }

        [Fact]
        public async Task AddProductAsync_ShouldFail_WhenCartIsNull()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var quantity = _fixture.Create<int>();
            var cart = _fixture.Create<Cart>();
            var product = _fixture.Create<Product>();
            var expectedException = new KeyNotFoundException($"{typeof(Cart)} with id {cart.Id} was not found");

            _productRepository.GetAsync(product.Id).Returns(product);

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.AddProductAsync(cart.Id, product.Id, quantity));

            // Assert
            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldFail_WhenProductIsNull()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var quantity = _fixture.Create<int>();
            var cart = _fixture.Create<Cart>();
            var product = _fixture.Create<Product>();
            var expectedException = new KeyNotFoundException($"{typeof(Product)} with id {product.Id} was not found");

            _cartRepository.GetAsync(cart.Id).Returns(cart);

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.AddProductAsync(cart.Id, product.Id, quantity));

            // Assert
            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldFail_WhenCartIsEmptyAndProductQuantityExceedsAvailability()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var cart = _fixture.Create<Cart>();
            cart.CartProducts = new List<CartProduct>();

            var product = _fixture.Create<Product>();
            var requestedQuantity = _fixture.Create<int>() + product.AvailableQuantity;
            var expectedException = new ExceededProductQuantityException(product.Name, product.PackageType,
                product.AvailableQuantity, requestedQuantity);

            _cartRepository.GetAsync(cart.Id).Returns(cart);
            _productRepository.GetAsync(product.Id).Returns(product);

            // Act
            var result = await Assert.ThrowsAsync<ExceededProductQuantityException>(async () => await _sut.AddProductAsync(cart.Id, product.Id, requestedQuantity));

            // Assert
            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldFail_WhenCartHasProductAndProductQuantityExceedsAvailability()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var cart = _fixture.Create<Cart>();
            var product = _fixture.Create<Product>();
            cart.CartProducts.Add(new CartProduct
            {
                ProductId = product.Id,
                CartId = cart.Id,
                Quantity = product.AvailableQuantity
            });
            var requestedQuantity = _fixture.Create<int>() + product.AvailableQuantity;
            var expectedResult = new ExceededProductQuantityException(product.Name, product.PackageType,
                product.AvailableQuantity, requestedQuantity);

            _cartRepository.GetAsync(cart.Id).Returns(cart);
            _productRepository.GetAsync(product.Id).Returns(product);

            // Act
            var result = await Assert.ThrowsAsync<ExceededProductQuantityException>(async () => await _sut.AddProductAsync(cart.Id, product.Id, requestedQuantity));

            // Assert
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldSucceed_WhenCartProductExistAndProductsAreAvailable()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var cart = _fixture.Create<Cart>();
            var product = _fixture.Create<Product>();

            cart.CartProducts.Add(new CartProduct()
            {
                ProductId = product.Id,
                Quantity = 1,
                CartId = cart.Id
            });

            var requestedQuantity = _fixture.Create<int>();
            var expectedQuantity = requestedQuantity + 1;
            product.AvailableQuantity = expectedQuantity + 2;

            _cartRepository.GetAsync(cart.Id).Returns(cart);
            _productRepository.GetAsync(product.Id).Returns(product);

            // Act
            await _sut.AddProductAsync(cart.Id, product.Id, requestedQuantity);

            // Assert
            await _cartRepository.Received().UpdateProductAsync(Arg.Is<CartProduct>(p =>
                p.CartId == cart.Id && p.Quantity == expectedQuantity &&
                p.ProductId == product.Id));
        }

        [Fact]
        public async Task AddProductAsync_ShouldSucceed_WhenCartIsEmptyAndProductsAreAvailable()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var requestedQuantity = _fixture.Create<int>();
            var cart = _fixture.Create<Cart>();
            cart.CartProducts = new List<CartProduct>();
            var product = _fixture.Create<Product>();

            _cartRepository.GetAsync(cart.Id).Returns(cart);
            _productRepository.GetAsync(product.Id).Returns(product);

            // Act
            await _sut.AddProductAsync(cart.Id, product.Id, requestedQuantity);

            // Assert
            await _cartRepository.Received().AddProductAsync(Arg.Is<CartProduct>(p =>
                p.CartId == cart.Id && p.Quantity == requestedQuantity &&
                p.ProductId == product.Id));
        }

        [Fact]
        public async Task EmptyCartAsync_ShouldFail_WhenCartIsNull()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var cart = _fixture.Create<Cart>();
            var expectedException = new KeyNotFoundException($"{typeof(Cart)} with id {cart.Id} was not found");

            // Act
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _sut.EmptyCartAsync(cart.Id));

            // Assert
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact]
        public async Task EmptyCartAsync_ShouldSucceed_WhenCartExists()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var cart = _fixture.Create<Cart>();
            var cartProductsToDelete = cart.CartProducts.Select(_ => _.Id);

            _cartRepository.GetAsync(cart.Id).Returns(cart);

            // Act
            await _sut.EmptyCartAsync(cart.Id);

            // Assert
            await _cartRepository.Received()
                .DeleteProductsAsync(Arg.Is<ICollection<CartProduct>>(p =>
                    !cartProductsToDelete.Except(p.Select(_ => _.Id)).Any()));
        }
    }
}