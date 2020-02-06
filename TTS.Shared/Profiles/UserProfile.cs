using AutoMapper;
using TTS.DAL.Entities;
using TTS.Shared.Models.User;

namespace TTS.Shared.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x=>x.Name,
                    opts=>
                        opts.MapFrom(src=>$"{src.FirstName} {src.LastName}"))
                .ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserEditDto>().ReverseMap();
            CreateMap<User, UserDetailsDto>().ReverseMap();
        }
    }
}