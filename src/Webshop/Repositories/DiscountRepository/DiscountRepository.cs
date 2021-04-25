using System.Threading.Tasks;
using AutoMapper;
using Webshop.Domain.DTOs.Discount;
using Webshop.Domain.Models;

namespace Webshop.Repositories.DiscountRepository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly WebshopContext _dbContext;
        private readonly IMapper _mapper;

        public DiscountRepository(WebshopContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<DiscountDto> CreateAsync(CreateDiscount createDiscount)
        {
            var discount = _mapper.Map<Discount>(createDiscount);
            _dbContext.Add(discount);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<DiscountDto>(discount);
        }

        public Task UpdateAsync(UpdateDiscount updateDiscount)
        {
            var discount = _mapper.Map<Discount>(updateDiscount);
            _dbContext.Update(discount);
            return _dbContext.SaveChangesAsync();
        }
    }
}