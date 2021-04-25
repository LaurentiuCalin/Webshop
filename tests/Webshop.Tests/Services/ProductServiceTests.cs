using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Core.Configurations;
using Webshop.Domain.DTOs.Product;
using Webshop.Domain.Models;
using Webshop.Repositories.ProductRepository;
using Webshop.Services.DatetimeService;
using Webshop.Services.ProductService;
using Webshop.Utils.Exceptions;
using Xunit;

namespace Webshop.Tests.Unit.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _sut;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();
        private readonly IFixture _fixture = new Fixture();

        public ProductServiceTests()
        {
            var mappingProfiles = new MappingProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfiles));
            _mapper = new Mapper(configuration);
            _sut = new ProductService(_productRepository, _mapper, _dateTimeService);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldSucceed()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var products = _fixture.CreateMany<Product>().ToList();
            var expectedResult = _mapper.Map<List<ProductOverview>>(products);
            _productRepository.GetAsync().Returns(products);

            // Act
            var result = await _sut.GetProductsAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task SubtractAvailabilityAsync_ShouldFail_WhenValueToSubtractIsBiggerThanProductAvailableValue()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var subtractProductAvailabilities = _fixture.Create<IReadOnlyCollection<SubtractProductAvailability>>();
            var product = _fixture.Create<Product>();
            product.Id = subtractProductAvailabilities.First().Id;
            product.AvailableQuantity = 0;

            var expectedResult = new ExceededProductQuantityException(product.Name, product.PackageType,
                product.AvailableQuantity, subtractProductAvailabilities.First().SubtractQuantity);

            _productRepository.GetAsync(subtractProductAvailabilities.Select(_ => _.Id).ToList())
                .ReturnsForAnyArgs(new List<Product> { product });

            // Act
            var result = await Assert.ThrowsAnyAsync<ExceededProductQuantityException>(async () =>
                await _sut.SubtractAvailabilityAsync(subtractProductAvailabilities));

            // Assert
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task SubtractAvailabilityAsync_ShouldSucceed()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var products = _fixture.Create<ICollection<Product>>();
            var subtractProductAvailabilities = products.Select(_ => new SubtractProductAvailability()
            {
                Id = _.Id,
                SubtractQuantity = _.AvailableQuantity
            }).ToList();

            _productRepository.GetAsync(new List<int>()).ReturnsForAnyArgs(products);

            // Act
            await _sut.SubtractAvailabilityAsync(subtractProductAvailabilities);

            // Assert
            await _productRepository.Received(1).UpdateAsync(products);
        }
    }
}