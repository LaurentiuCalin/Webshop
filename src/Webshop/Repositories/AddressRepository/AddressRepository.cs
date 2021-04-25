using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Models;

namespace Webshop.Repositories.AddressRepository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly WebshopContext _dbContext;

        public AddressRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Address> CreateAsync(Address address)
        {
            _dbContext.Add(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }
    }
}