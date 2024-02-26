using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Core.Mapping
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterModel, User>()
                .ForMember(x => x.UserName, opts => opts.MapFrom(s => s.Email));
        }
    }
}
