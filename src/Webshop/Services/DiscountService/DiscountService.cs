using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Webshop.Domain.DTOs.Discount;
using Webshop.Repositories.DiscountRepository;
using Webshop.Repositories.MembershipRepository;
using Webshop.Repositories.ProductRepository;
using Webshop.Utils.Exceptions;
using Webshop.Utils.Extensions;

namespace Webshop.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IValidator<CreateDiscount> _createDiscountValidator;
        private readonly IValidator<UpdateDiscount> _updateDiscountValidator;

        public DiscountService(
            IMapper mapper,
            IProductRepository productRepository,
            IDiscountRepository discountRepository,
            IMembershipRepository membershipRepository,
            IValidator<CreateDiscount> createDiscountValidator,
            IValidator<UpdateDiscount> updateDiscountValidator)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _membershipRepository = membershipRepository;
            _createDiscountValidator = createDiscountValidator;
            _updateDiscountValidator = updateDiscountValidator;
        }

        public async Task<DiscountDto> CreateProductDiscountAsync(CreateProductDiscount createProductDiscount)
        {
            var validationResult = await _createDiscountValidator.ValidateAsync(createProductDiscount.Discount);

            if (!validationResult.IsValid)
            {
                throw new InvalidDiscountConfigurationException(createProductDiscount.Discount.Name,
                    validationResult.Errors);
            }

            var product = await _productRepository.GetAsync(createProductDiscount.ProductId);
            product.ThrowIfNull(createProductDiscount.ProductId);

            var discount = await _discountRepository.CreateAsync(createProductDiscount.Discount);

            product.DiscountId = discount.Id;
            await _productRepository.UpdateAsync(product);
            return _mapper.Map<DiscountDto>(discount);
        }

        public async Task<DiscountDto> CreateMembershipDiscountAsync(CreateMembershipDiscount createProductDiscount)
        {
            var validationResult = await _createDiscountValidator.ValidateAsync(createProductDiscount.Discount);

            if (!validationResult.IsValid)
            {
                throw new InvalidDiscountConfigurationException(createProductDiscount.Discount.Name,
                    validationResult.Errors);
            }

            var membership = await _membershipRepository.GetAsync(createProductDiscount.MembershipId);
            membership.ThrowIfNull(createProductDiscount.MembershipId);

            var discount = await _discountRepository.CreateAsync(createProductDiscount.Discount);

            membership.DiscountId = discount.Id;
            await _membershipRepository.UpdateAsync(membership);
            return _mapper.Map<DiscountDto>(discount);
        }

        public async Task UpdateAsync(UpdateDiscount updateDiscount)
        {
            var validationResult = await _updateDiscountValidator.ValidateAsync(updateDiscount);

            if (!validationResult.IsValid)
            {
                throw new InvalidDiscountConfigurationException(updateDiscount.Name,
                    validationResult.Errors);
            }

            await _discountRepository.UpdateAsync(updateDiscount);
        }
    }
}