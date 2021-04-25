using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Models;

namespace Webshop.Repositories.MembershipRepository
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly WebshopContext _dbContext;

        public MembershipRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Membership>> GetAsync()
        {
            return _dbContext.Memberships.ToListAsync();
        }

        public Task<Membership> GetAsync(string label)
        {
            return _dbContext.Memberships.FirstOrDefaultAsync(_ => string.Equals(_.Label, label));
        }

        public Task<Membership> GetAsync(int id)
        {
            return _dbContext.Memberships.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public Task UpdateAsync(Membership membership)
        {
            _dbContext.Update(membership);
            return _dbContext.SaveChangesAsync();
        }
    }
}