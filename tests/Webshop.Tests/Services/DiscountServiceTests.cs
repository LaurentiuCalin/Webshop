using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Webshop.Core.Configurations;
using Webshop.Domain.DTOs.Discount;
using Webshop.Domain.Models;
using Webshop.Repositories.DiscountRepository;
using Webshop.Repositories.MembershipRepository;
using Webshop.Repositories.ProductRepository;
using Webshop.Services.DiscountService;
using Xunit;

namespace Webshop.Tests.Unit.Services
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _sut;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IDiscountRepository _discountRepository = Substitute.For<IDiscountRepository>();
        private readonly IMembershipRepository _membershipRepository = Substitute.For<IMembershipRepository>();
        private readonly IMapper _mapper;
        private readonly IFixture _fixture = new Fixture();

        public DiscountServiceTests()
        {
            var mappingProfiles = new MappingProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfiles));
            _mapper = new Mapper(configuration);
            _sut = new DiscountService(_mapper, _productRepository, _discountRepository, _membershipRepository);
        }

        [Fact]
        public async Task CreateProductDiscountAsync_ShouldFail_WhenProductIsNull()
        {
            // Arrange
            var productDiscount = _fixture.Create<CreateProductDiscount>();
            var expectedException = new KeyNotFoundException($"{typeof(Product)} with id {productDiscount.ProductId} was not found");

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.CreateProductDiscountAsync(productDiscount));

            // Assert
            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task CreateProductDiscountAsync_ShouldSucceed_WhenProductExists()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var createProductDiscount = _fixture.Create<CreateProductDiscount>();
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

            await _discountRepository.Received().CreateAsync(Arg.Is<CreateDiscount>(p =>
                p.IsActive == createProductDiscount.Discount.IsActive &&
                p.Name == createProductDiscount.Discount.Name &&
                p.Percentage == createProductDiscount.Discount.Percentage));

            await _productRepository.Received().UpdateAsync(Arg.Is<Product>(p =>
                p.Id == product.Id && p.Name == product.Name && p.DiscountId == result.Id));
        }

        [Fact]
        public async Task CreateMembershipDiscountAsync_ShouldFail_WhenMembershipIsNull()
        {
            // Arrange
            var createMembershipDiscount = _fixture.Create<CreateMembershipDiscount>();
            var expectedException = new KeyNotFoundException($"{typeof(Membership)} with id {createMembershipDiscount.MembershipId} was not found");

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sut.CreateMembershipDiscountAsync(createMembershipDiscount));

            // Assert
            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task CreateMembershipDiscountAsync_ShouldSucceed_WhenMembershipExists()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var createMembershipDiscount = _fixture.Create<CreateMembershipDiscount>();
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

            await _discountRepository.Received().CreateAsync(Arg.Is<CreateDiscount>(p =>
                p.IsActive == createMembershipDiscount.Discount.IsActive &&
                p.Name == createMembershipDiscount.Discount.Name &&
                p.Percentage == createMembershipDiscount.Discount.Percentage));

            await _membershipRepository.Received().UpdateAsync(Arg.Is<Membership>(p =>
                p.Id == membership.Id && p.Label == membership.Label && p.DiscountId == result.Id));
        }

        [Fact]
        public async Task UpdateAsync_ShouldSucceed()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var updateDiscount = _fixture.Create<UpdateDiscount>();

            // Act
            await _sut.UpdateAsync(updateDiscount);

            // Assert
            await _discountRepository.Received().UpdateAsync(Arg.Is<UpdateDiscount>(p =>
                p.IsActive == updateDiscount.IsActive &&
                p.Name == updateDiscount.Name &&
                p.Percentage == updateDiscount.Percentage &&
                p.Id == updateDiscount.Id));
        }
    }
}