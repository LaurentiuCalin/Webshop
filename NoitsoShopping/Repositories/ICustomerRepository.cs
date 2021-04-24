using System.Threading.Tasks;

namespace NoitsoShopping.Repositories
{
    public interface ICustomerRepository
    {
        Task AssignMembershipAsync(int id, int membershipId);
    }
}