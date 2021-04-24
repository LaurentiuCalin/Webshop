using AutoMapper;
using NoitsoShopping.Domain.DTOs;
using NoitsoShopping.Domain.DTOs.Discount;
using NoitsoShopping.Repositories.DiscountRepository;
using NoitsoShopping.Repositories.ProductRepository;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NoitsoShopping.Domain.DTOs.Product;

namespace NoitsoShopping.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            IDiscountRepository discountRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var products = await _productRepository.GetAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDiscount> CreateDiscountAsync(CreateProductDiscount createProductDiscount)
        {
            var product = await _productRepository.GetAsync(createProductDiscount.ProductId);

            if (product is null)
            {
                throw new NoNullAllowedException("The specified product does not exist.");
            }

            var discount = await _discountRepository.CreateAsync(_mapper.Map<CreateDiscount>(createProductDiscount));
            //await _productRepository.UpdateAsync(new UpdateProduct
            //{
            //    Name = product.Name,
            //    AvailableQuantity = product.AvailableQuantity,
            //    CategoryId = product.Category.Id,
            //    DiscountId = discount.Id,
            //    PackageQuantity = product.PackageQuantity,
            //    PackageType = product.PackageType,
            //    Price = product.Price
            //});
            return null;
        }
    }
}