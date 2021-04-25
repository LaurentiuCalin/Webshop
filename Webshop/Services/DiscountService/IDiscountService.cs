using System.Threading.Tasks;
using Webshop.Domain.DTOs.Discount;

namespace Webshop.Services.DiscountService
{
    public interface IDiscountService
    {
        Task<DiscountDto> CreateProductDiscountAsync(CreateProductDiscount createProductDiscount);
        Task<DiscountDto> CreateMembershipDiscountAsync(CreateMembershipDiscount createProductDiscount);
        Task UpdateAsync(UpdateDiscount updateDiscount);
    }
}