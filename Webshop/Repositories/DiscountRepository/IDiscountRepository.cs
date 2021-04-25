using System.Threading.Tasks;
using Webshop.Domain.DTOs.Discount;

namespace Webshop.Repositories.DiscountRepository
{
    public interface IDiscountRepository
    {
        Task<DiscountDto> CreateAsync(CreateDiscount createDiscount);
        Task UpdateAsync(UpdateDiscount updateDiscount);
    }
}