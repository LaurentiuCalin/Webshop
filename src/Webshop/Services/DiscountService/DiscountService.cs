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
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMembershipRepository _membershipRepository;

        public DiscountService(
            IMapper mapper,
            IProductRepository productRepository,
            IDiscountRepository discountRepository,
            IMembershipRepository membershipRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _discountRepository = discountRepository;
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