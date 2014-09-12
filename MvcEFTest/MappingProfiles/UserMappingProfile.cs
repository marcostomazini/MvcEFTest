using AutoMapper;
using MvcEFTest.Entities;
using MvcEFTest.Models;
using MvcEFTest.Views;

namespace MvcEFTest.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserViewModel>().ReverseMap();

            Mapper.CreateMap<Phone, PhoneViewModel>().ReverseMap();
        }
    }
}