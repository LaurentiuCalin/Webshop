using NoitsoShopping.Domain.Models;
using NoitsoShopping.Repositories.CartRepository;
using NoitsoShopping.Repositories.ProductRepository;
using NoitsoShopping.Utils.Exceptions;
using NoitsoShopping.Utils.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace NoitsoShopping.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task AddProductAsync(int cartId, int productId, int quantity)
        {
            var cart = await _cartRepository.GetAsync(cartId);
            cart.ThrowIfNull(cartId);

            var product = await _productRepository.GetAsync(productId);
            product.ThrowIfNull(productId);

            var cartProduct = cart.CartProducts.SingleOrDefault(_ => _.ProductId == product.Id);

            if (cartProduct is null)
            {
                await HandleNewCartProduct(cart, product, quantity);
                return;
            }

            await HandleExistingCartProduct(cartProduct, product, quantity);
        }

        private Task HandleNewCartProduct(Cart cart, Product product, int requestedQuantity)
        {
            var cartProduct = new CartProduct
            {
                Quantity = requestedQuantity,
                CartId = cart.Id,
                ProductId = product.Id
            };

            if (cartProduct.Quantity > product.AvailableQuantity)
            {
                throw new ExceededProductQuantityException(product.Name, product.PackageType, product.AvailableQuantity, requestedQuantity);
            }

            return _cartRepository.AddProductAsync(cartProduct);
        }

        private Task HandleExistingCartProduct(CartProduct cartProduct, Product product, int requestedQuantity)
        {
            cartProduct.Quantity += requestedQuantity;

            if (cartProduct.Quantity > product.AvailableQuantity)
            {
                throw new ExceededProductQuantityException(product.Name, product.PackageType, product.AvailableQuantity, requestedQuantity);
            }

            return _cartRepository.UpdateProductAsync(cartProduct);
        }
    }
}