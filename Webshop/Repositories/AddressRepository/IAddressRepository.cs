using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.AddressRepository
{
    public interface IAddressRepository
    {
        Task<Address> CreateAsync(Address address);
        Task<Address> GetAsync(int id);
    }
}