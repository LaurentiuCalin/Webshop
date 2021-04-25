using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Models;

namespace Webshop.Repositories.PaymentRepository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly WebshopContext _dbContext;

        public PaymentRepository(WebshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _dbContext.Add(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }
    }
}