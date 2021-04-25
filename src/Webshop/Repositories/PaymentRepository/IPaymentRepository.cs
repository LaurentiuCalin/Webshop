using System.Threading.Tasks;
using Webshop.Domain.Models;

namespace Webshop.Repositories.PaymentRepository
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
    }
}