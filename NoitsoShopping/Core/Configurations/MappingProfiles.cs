using AutoMapper;
using NoitsoShopping.Domain.DTOs;
using NoitsoShopping.Domain.Models;

namespace NoitsoShopping.Core.Configurations
{
    internal class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
        }
    }
}