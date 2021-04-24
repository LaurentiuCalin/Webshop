using System.Threading.Tasks;
using NoitsoShopping.Domain.DTOs;
using NoitsoShopping.Domain.DTOs.Discount;

namespace NoitsoShopping.Repositories.DiscountRepository
{
    public interface IDiscountRepository
    {
        Task<DiscountDto> CreateAsync(CreateDiscount createDiscount);
        Task UpdateAsync(UpdateDiscount updateDiscount);
    }
}