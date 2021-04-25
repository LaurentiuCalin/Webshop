using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Webshop.Domain.DTOs.Discount;
using Webshop.Domain.DTOs.Product;
using Webshop.Domain.Models;
using Webshop.Repositories.ProductRepository;
using Webshop.Services.DatetimeService;
using Webshop.Utils.Extensions;

namespace Webshop.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            IDateTimeService dateTimeService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public async Task<List<ProductOverview>> GetProductsAsync()
        {
            var products = await _productRepository.GetAsync();
            return products.Select(ToProductOverview).ToList();
        }

        public async Task SubtractAvailabilityAsync(
            IReadOnlyCollection<SubtractProductAvailability> productAvailabilities)
        {
            var products = await _productRepository.GetAsync(productAvailabilities.Select(_ => _.Id).ToList());

            Parallel.ForEach(products, product =>
            {
                var valueToSubtract = productAvailabilities.Single(_ => _.Id == product.Id).Id;
                product.AvailableQuantity -= valueToSubtract;
            });

            await _productRepository.UpdateAsync(products);
        }

        private ProductOverview ToProductOverview(Product product)
        {
            var productOverview = _mapper.Map<ProductOverview>(product);

            if (!product.Discount.IsAvailable(_dateTimeService.GetCurrentUtc())) return productOverview;

            productOverview.DiscountOverview = _mapper.Map<DiscountOverview>(product.Discount);
            return productOverview;
        }
    }
}