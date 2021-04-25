using System.Threading.Tasks;
using AutoMapper;
using Webshop.Domain.DTOs.Discount;
using Webshop.Repositories.DiscountRepository;
using Webshop.Repositories.MembershipRepository;
using Webshop.Repositories.ProductRepository;
using Webshop.Utils.Extensions;

namespace Webshop.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IProductRepository _productRepository;

        public DiscountService(
            IProductRepository productRepository,
            IDiscountRepository discountRepository,
            IMapper mapper,
            IMembershipRepository membershipRepository)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _mapper = mapper;
            _membershipRepository = membershipRepository;
        }

        public async Task<DiscountDto> CreateProductDiscountAsync(CreateProductDiscount createProductDiscount)
        {
            var product = await _productRepository.GetAsync(createProductDiscount.ProductId);
            product.ThrowIfNull(createProductDiscount.ProductId);

            var discount = await _discountRepository.CreateAsync(createProductDiscount.Discount);

            product.DiscountId = discount.Id;
            await _productRepository.UpdateAsync(product);
            return _mapper.Map<DiscountDto>(discount);
        }

        public async Task<DiscountDto> CreateMembershipDiscountAsync(CreateMembershipDiscount createProductDiscount)
        {
            var membership = await _membershipRepository.GetAsync(createProductDiscount.MembershipId);
            membership.ThrowIfNull(createProductDiscount.MembershipId);

            var discount = await _discountRepository.CreateAsync(createProductDiscount.Discount);

            membership.DiscountId = discount.Id;
            await _membershipRepository.UpdateAsync(membership);
            return _mapper.Map<DiscountDto>(discount);
        }

        public Task UpdateAsync(UpdateDiscount updateDiscount)
        {
            return _discountRepository.UpdateAsync(updateDiscount);
        }
    }
}