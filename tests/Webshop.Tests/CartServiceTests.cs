using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Domain.Models;
using Webshop.Repositories.CartRepository;
using Webshop.Repositories.ProductRepository;
using Webshop.Services.CartService;
using Webshop.Utils.Exceptions;
using Webshop.Utils.Extensions;
using Xunit;

namespace Webshop.Tests.Unit
{
    public class CartServiceTests
    {
        private readonly CartService _sut;
        private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

        public CartServiceTests()
        {
            _sut = new CartService(_cartRepository, _productRepository);
        }

        [Fact]
        public async Task AddProductAsync_ShouldFail_WhenCartIsNull()
        {
            // Arrange
            const int quantity = 10;
            var cart = new Cart { Id = 1 };
            var product = new Product { Id = 1 };

            _productRepository.GetAsync(product.Id).Returns(product);

            var expectedException = new Exception();

            try
            {
                Cart nullCart = null;
                nullCart.ThrowIfNull(cart.Id);
            }
            catch (Exception e)
            {
                expectedException = e;
            }

            // Act
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.AddProductAsync(cart.Id, product.Id, quantity));

            // Assert
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldFail_WhenProductIsNull()
        {
            // Arrange
            const int quantity = 10;
            var cart = new Cart { Id = 1 };

            _cartRepository.GetAsync(cart.Id).Returns(cart);

            var product = new Product { Id = 1 };

            var expectedException = new Exception();
            try
            {
                Product nullProduct = null;
                nullProduct.ThrowIfNull(product.Id);
            }
            catch (Exception e)
            {
                expectedException = e;
            }

            // Act
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.AddProductAsync(cart.Id, product.Id, quantity));

            // Assert
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldFail_WhenCartIsEmptyAndProductQuantityExceedsAvailability()
        {
            // Arrange
            const int quantity = 10;
            var cart = new Cart { Id = 1 };
            var product = new Product { Id = 1, Name = "Banana", PackageType = "Bulk", AvailableQuantity = 6 };

            _cartRepository.GetAsync(cart.Id).Returns(cart);
            _productRepository.GetAsync(product.Id).Returns(product);

            var expectedException = new ExceededProductQuantityException(product.Name, product.PackageType,
                product.AvailableQuantity, quantity);

            // Act
            var exception = await Assert.ThrowsAsync<ExceededProductQuantityException>(async () => await _sut.AddProductAsync(cart.Id, product.Id, quantity));

            // Assert
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldFail_WhenCartHasProductAndProductQuantityExceedsAvailability()
        {
            // Arrange
            const int requestedQuantity = 10;
            var product = new Product { Id = 1, Name = "Banana", PackageType = "Bulk", AvailableQuantity = 15 };

            var cart = new Cart
            {
                Id = 1,
                CartProducts = new List<CartProduct>()
                {
                    new ()
                    {
                        Id = 1,
                        CartId = 1,
                        ProductId = product.Id,
                        Quantity = requestedQuantity
                    }
                }
            };

            _cartRepository.GetAsync(cart.Id).Returns(cart);
            _productRepository.GetAsync(product.Id).Returns(product);

            var expectedException = new ExceededProductQuantityException(product.Name, product.PackageType,
                product.AvailableQuantity, requestedQuantity);

            // Act
            var exception = await Assert.ThrowsAsync<ExceededProductQuantityException>(async () => await _sut.AddProductAsync(cart.Id, product.Id, requestedQuantity));

            // Assert
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldSucceed_WhenCartProductExistAndHasProductsAreAvailable()
        {
            // Arrange
            const int requestedQuantity = 10;
            var product = new Product { Id = 1, Name = "Banana", PackageType = "Bulk", AvailableQuantity = 20 };

            var cart = new Cart
            {
                Id = 1,
                CartProducts = new List<CartProduct>()
                {
                    new ()
                    {
                        Id = 1,
                        CartId = 1,
                        ProductId = product.Id,
                        Quantity = 0
                    }
                }
            };

            _cartRepository.GetAsync(cart.Id).Returns(cart);
            _productRepository.GetAsync(product.Id).Returns(product);
            // Act
            await _sut.AddProductAsync(cart.Id, product.Id, requestedQuantity);

            // Assert
            Assert.Equal(cart.CartProducts.First(_ => _.Id == 1).Quantity, requestedQuantity);
        }

        [Fact]
        public async Task AddProductAsync_ShouldSucceed_WhenCartIsEmptyAndProductsAreAvailable()
        {
            // Arrange
            const int requestedQuantity = 10;
            var product = new Product { Id = 1, Name = "Banana", PackageType = "Bulk", AvailableQuantity = 20 };
            var cart = new Cart { Id = 1 };

            var cartProduct = new CartProduct
            {
                CartId = cart.Id,
                Quantity = requestedQuantity,
                ProductId = product.Id
            };

            _cartRepository.GetAsync(cart.Id).Returns(cart);
            _productRepository.GetAsync(product.Id).Returns(product);

            // Act
            await _sut.AddProductAsync(cart.Id, product.Id, requestedQuantity);

            // Assert
            await _cartRepository.Received().AddProductAsync(Arg.Is<CartProduct>(p =>
                p.CartId == cartProduct.CartId && p.Quantity == cartProduct.Quantity &&
                p.ProductId == cartProduct.ProductId));
        }

        [Fact]
        public async Task EmptyCartAsync_ShouldFail_WhenCartIsNull()
        {
            // Arrange
            var cart = new Cart { Id = 1 };
            var expectedException = new Exception();

            try
            {
                Cart nullCart = null;
                nullCart.ThrowIfNull(cart.Id);
            }
            catch (Exception e)
            {
                expectedException = e;
            }

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
            var firstCartProduct = new CartProduct
            {
                Id = 1,
                CartId = 1,
                ProductId = 1,
                Quantity = 10
            };
            var secondCartProduct = new CartProduct
            {
                Id = 2,
                CartId = 1,
                ProductId = 1,
                Quantity = 10
            };
            var cart = new Cart
            {
                Id = 1,
                CartProducts = new List<CartProduct> { firstCartProduct, secondCartProduct }
            };
            _cartRepository.GetAsync(cart.Id).Returns(cart);

            // Act
            await _sut.EmptyCartAsync(cart.Id);

            // Assert
            await _cartRepository.Received().DeleteProductsAsync(Arg.Is<ICollection<CartProduct>>(p =>
                p.First().Id == firstCartProduct.Id && p.Last().Id == secondCartProduct.Id));
        }
    }
}