using AutoMapper;
using TTS.DAL.Entities;
using TTS.Shared.Models.User;

namespace TTS.Web.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(x=>x.Name,
                    opts=>
                        opts.MapFrom(src=>$"{src.FirstName} {src.LastName}"))
                .ReverseMap();
            CreateMap<User, UserCreateModel>().ReverseMap();
            CreateMap<User, UserEditModel>().ReverseMap();
            CreateMap<User, UserDetailsModel>().ReverseMap();
        }
    }
}