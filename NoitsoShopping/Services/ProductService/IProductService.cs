using NoitsoShopping.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoitsoShopping.Domain.DTOs.Product;

namespace NoitsoShopping.Services.ProductService
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts();
        Task<ProductDiscount> CreateDiscountAsync(CreateProductDiscount createProductDiscount);
    }
}