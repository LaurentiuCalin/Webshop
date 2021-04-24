using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoitsoShopping.Domain.Models;

namespace NoitsoShopping.Repositories.MembershipRepository
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly WebshopContext _dbContext;

        public MembershipRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Membership>> GetMemberships()
        {
            return _dbContext.Memberships.ToListAsync();
        }

        public Task<Membership> GetMembershipAsync(string label)
        {
            return _dbContext.Memberships.FirstOrDefaultAsync(_ => string.Equals(_.Label, label));
        }
    }
}