using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.MembershipRepository
{
    public interface IMembershipRepository
    {
        Task<List<Membership>> GetAsync();
        Task<Membership> GetAsync(string label);
        Task<Membership> GetAsync(int id);
        Task UpdateAsync(Membership membership);
    }
}