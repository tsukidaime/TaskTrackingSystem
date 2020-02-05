using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TTS.Shared.Models.Role;

namespace TTS.Web.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole<Guid>, RoleDto>().ReverseMap();
        }
    }
}