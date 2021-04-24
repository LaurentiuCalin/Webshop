using System.Threading.Tasks;
using AutoMapper;
using NoitsoShopping.Domain.DTOs;
using NoitsoShopping.Domain.DTOs.Discount;
using NoitsoShopping.Domain.Models;

namespace NoitsoShopping.Repositories.DiscountRepository
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
            throw new System.NotImplementedException();
        }


    }
}