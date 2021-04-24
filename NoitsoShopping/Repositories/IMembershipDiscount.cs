using System.Threading.Tasks;
using NoitsoShopping.Domain.DTOs;
using NoitsoShopping.Domain.DTOs.SaleType;

namespace NoitsoShopping.Repositories
{
    public interface IMembershipSale
    {
        Task<MembershipSaleDto> CreateAsync(int membershipId, int saleId, BaseSaleConfiguration configuration);
    }
}