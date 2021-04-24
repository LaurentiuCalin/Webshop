using NoitsoShopping.Utils.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoitsoShopping.Domain.DTOs.SaleType;

namespace NoitsoShopping.Repositories
{
    public interface ISaleTypeRepository
    {
        Task<ICollection<SaleTypeDto>> GetAsync();
        Task<SaleTypeDto> GetAsync(SaleType saleType);
    }
}