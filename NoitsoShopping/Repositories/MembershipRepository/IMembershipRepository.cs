using System.Collections.Generic;
using System.Threading.Tasks;
using NoitsoShopping.Domain.Models;

namespace NoitsoShopping.Repositories.MembershipRepository
{
    public interface IMembershipRepository
    {
        Task<List<Membership>> GetMemberships();
        Task<Membership> GetMembershipAsync(string label);
    }
}