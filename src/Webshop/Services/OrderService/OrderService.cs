using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Webshop.Domain.DTOs;
using Webshop.Domain.DTOs.Cart;
using Webshop.Domain.DTOs.Order;
using Webshop.Domain.DTOs.Product;
using Webshop.Domain.Models;
using Webshop.Repositories.OrderRepository;
using Webshop.Services.CartService;
using Webshop.Services.ProductService;

namespace Webshop.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;

        public OrderService(
            IOrderRepository orderRepository,
            IMapper mapper,
            ICartService cartService,
            IProductService productService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<OrderDto> CreateAsync(CartOverview cartOverview, AddressDto address, PaymentDto payment)
        {
            var order = await _orderRepository.CreateAsync(new Order
            {
                AddressId = address.Id,
                PaymentId = payment.Id,
                UserId = cartOverview.User.Id,
                OrderProducts = cartOverview.Products.Select(ToOrderProduct).ToList()
            });

            await _cartService.EmptyCartAsync(cartOverview.CartId);
            await _productService.SubtractAvailabilityAsync(
                order.OrderProducts.Select(ToSubtractProductAvailability)
                    .ToList());

            return _mapper.Map<OrderDto>(order);
        }

        private static SubtractProductAvailability ToSubtractProductAvailability(OrderProduct orderProduct)
        {
            return new()
            {
                Id = orderProduct.Id,
                SubtractQuantity = orderProduct.Quantity
            };
        }

        private static OrderProduct ToOrderProduct(CartProductOverview cartProductOverview)
        {
            return new()
            {
                Price = cartProductOverview.FinalPrice,
                ProductId = cartProductOverview.Id,
                Quantity = cartProductOverview.TotalQuantity
            };
        }
    }
}