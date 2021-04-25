using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Domain.DTOs.Product;

namespace Webshop.Services.ProductService
{
    public interface IProductService
    {
        Task<List<ProductOverview>> GetProductsAsync();
        Task SubtractAvailabilityAsync(IReadOnlyCollection<SubtractProductAvailability> productAvailabilities);
    }
}