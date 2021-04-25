using System.Threading.Tasks;
using Webshop.Domain.DTOs.Discount;
using Webshop.Domain.Models;

namespace Webshop.Repositories.DiscountRepository
{
    public interface IDiscountRepository
    {
        Task<Discount> CreateAsync(CreateDiscount createDiscount);
        Task UpdateAsync(UpdateDiscount updateDiscount);
    }
}