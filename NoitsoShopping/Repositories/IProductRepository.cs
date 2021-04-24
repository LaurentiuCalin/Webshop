using NoitsoShopping.Domain.Contracts;
using NoitsoShopping.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoitsoShopping.Domain.DTOs.SaleType;

namespace NoitsoShopping.Repositories
{
    public interface IProductRepository
    {
        Task<ICollection<ProductDto>> GetAsync(ProductsFilter filter);
        Task<ProductDto> CreateAsync(ProductCreateDto product);
        Task<ProductSaleDto> CreateSaleAsync(CreateProductSaleDto createProductSaleDto);
        Task UpdateSaleAsync(int productId, UpdateProductSaleDto updateProductSaleDto);
    }
}