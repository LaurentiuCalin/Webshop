using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Core.Configurations;
using Webshop.Domain.DTOs.Discount;
using Webshop.Domain.Models;
using Webshop.Repositories.DiscountRepository;
using Webshop.Repositories.MembershipRepository;
using Webshop.Repositories.ProductRepository;
using Webshop.Services.DiscountService;
using Webshop.Validators;
using Xunit;

namespace Webshop.Tests.Unit.Services
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _sut;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IDiscountRepository _discountRepository = Substitute.For<IDiscountRepository>();
        private readonly IMembershipRepository _membershipRepository = Substitute.For<IMembershipRepository>();
        private readonly IValidator<CreateDiscount> _createDiscountValidator = new CreateDiscountValidator();
        private readonly IValidator<UpdateDiscount> _updateDiscountValidator = new UpdateDiscountValidator();
        private readonly IMapper _mapper;
        private readonly IFixture _fixture = new Fixture();

        public DiscountServiceTests()
        {
            var mappingProfiles = new MappingProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfiles));
            _mapper = new Mapper(configuration);
            _sut = new DiscountService(_mapper, _productRepository, _discountRepository, _membershipRepository,
                _createDiscountValidator, _updateDiscountValidator);
        }

        [Fact]
        public async Task CreateProductDiscountAsync_ShouldFail_WhenProductIsNull()
        {
            // Arrange
            var createProductDiscount = _fixture.Create<CreateProductDiscount>();

            createProductDiscount.Discount.MaxQuantity = 20;
            createProductDiscount.Discount.MinQuantity = 10;
            createProductDiscount.Discount.Percentage = 14;
            createProductDiscount.Discount.ValidFrom = null;
            createProductDiscount.Discount.ValidUntil = null;

            var expectedResult = new KeyNotFoundException($"{typeof(Product)} with id {createProductDiscount.ProductId} was not found");

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.CreateProductDiscountAsync(createProductDiscount));

            // Assert
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task CreateProductDiscountAsync_ShouldSucceed_WhenProductExists()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var createProductDiscount = _fixture.Create<CreateProductDiscount>();
            createProductDiscount.Discount.MaxQuantity = 20;
            createProductDiscount.Discount.MinQuantity = 10;
            createProductDiscount.Discount.Percentage = 14;
            createProductDiscount.Discount.ValidFrom = null;
            createProductDiscount.Discount.ValidUntil = null;
            var product = _fixture.Create<Product>();
            createProductDiscount.ProductId = product.Id;

            var createdDiscount = _fixture.Create<Discount>();
            var expectedResult = _mapper.Map<DiscountDto>(createdDiscount);

            _productRepository.GetAsync(product.Id).Returns(product);
            _discountRepository.CreateAsync(createProductDiscount.Discount).Returns(createdDiscount);

            // Act
            var result = await _sut.CreateProductDiscountAsync(createProductDiscount);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);

            await _discountRepository.Received(1).CreateAsync(Arg.Is<CreateDiscount>(p =>
                p.IsActive == createProductDiscount.Discount.IsActive &&
                p.Name == createProductDiscount.Discount.Name &&
                p.Percentage == createProductDiscount.Discount.Percentage));

            await _productRepository.Received(1).UpdateAsync(Arg.Is<Product>(p =>
                p.Id == product.Id && p.Name == product.Name && p.DiscountId == result.Id));
        }

        [Fact]
        public async Task CreateMembershipDiscountAsync_ShouldFail_WhenMembershipIsNull()
        {
            // Arrange
            var createMembershipDiscount = _fixture.Create<CreateMembershipDiscount>();
            createMembershipDiscount.Discount.MaxQuantity = 20;
            createMembershipDiscount.Discount.MinQuantity = 10;
            createMembershipDiscount.Discount.Percentage = 14;
            createMembershipDiscount.Discount.ValidFrom = null;
            createMembershipDiscount.Discount.ValidUntil = null;
            var expectedResult = new KeyNotFoundException($"{typeof(Membership)} with id {createMembershipDiscount.MembershipId} was not found");

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.CreateMembershipDiscountAsync(createMembershipDiscount));

            // Assert
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task CreateMembershipDiscountAsync_ShouldSucceed_WhenMembershipExists()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var createMembershipDiscount = _fixture.Create<CreateMembershipDiscount>();
            createMembershipDiscount.Discount.MaxQuantity = 20;
            createMembershipDiscount.Discount.MinQuantity = 10;
            createMembershipDiscount.Discount.Percentage = 14;
            createMembershipDiscount.Discount.ValidFrom = null;
            createMembershipDiscount.Discount.ValidUntil = null;
            var membership = _fixture.Create<Membership>();
            createMembershipDiscount.MembershipId = membership.Id;

            var createdDiscount = _fixture.Create<Discount>();
            var expectedResult = _mapper.Map<DiscountDto>(createdDiscount);

            _membershipRepository.GetAsync(membership.Id).Returns(membership);
            _discountRepository.CreateAsync(createMembershipDiscount.Discount).Returns(createdDiscount);

            // Act
            var result = await _sut.CreateMembershipDiscountAsync(createMembershipDiscount);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);

            await _discountRepository.Received(1).CreateAsync(Arg.Is<CreateDiscount>(p =>
                p.IsActive == createMembershipDiscount.Discount.IsActive &&
                p.Name == createMembershipDiscount.Discount.Name &&
                p.Percentage == createMembershipDiscount.Discount.Percentage));

            await _membershipRepository.Received(1).UpdateAsync(Arg.Is<Membership>(p =>
                p.Id == membership.Id && p.Label == membership.Label && p.DiscountId == result.Id));
        }

        [Fact]
        public async Task UpdateAsync_ShouldSucceed()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var updateDiscount = _fixture.Create<UpdateDiscount>();
            updateDiscount.MaxQuantity = 20;
            updateDiscount.MinQuantity = 10;
            updateDiscount.Percentage = 14;
            updateDiscount.ValidFrom = null;
            updateDiscount.ValidUntil = null;

            // Act
            await _sut.UpdateAsync(updateDiscount);

            // Assert
            await _discountRepository.Received(1).UpdateAsync(Arg.Is<UpdateDiscount>(p =>
                p.IsActive == updateDiscount.IsActive &&
                p.Name == updateDiscount.Name &&
                p.Percentage == updateDiscount.Percentage &&
                p.Id == updateDiscount.Id));
        }
    }
}