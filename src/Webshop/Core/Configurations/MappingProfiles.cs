using AutoMapper;
using Webshop.Domain.DTOs;
using Webshop.Domain.DTOs.Cart;
using Webshop.Domain.DTOs.Discount;
using Webshop.Domain.DTOs.Product;
using Webshop.Domain.Models;

namespace Webshop.Core.Configurations
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Product, ProductOverview>()
                .ForMember(dest => dest.Category, src => src.MapFrom(_ => _.Category.Name));

            CreateMap<Product, ProductDto>();

            CreateMap<Product, CartProductOverview>()
                .ForMember(dest => dest.Category, src => src.MapFrom(_ => _.Category.Name));

            CreateMap<Discount, DiscountOverview>();
            CreateMap<Discount, DiscountDto>();
            CreateMap<DiscountDto,Discount>();
            CreateMap<UpdateDiscount, Discount>();
            CreateMap<CreateDiscount, Discount>();

            CreateMap<Address, AddressDto>();
            CreateMap<CreateAddress, Address>();

            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePayment, Payment>();
        }
    }
}