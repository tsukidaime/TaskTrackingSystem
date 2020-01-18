using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TTS.Shared.Models;

namespace TTS.Web.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole<Guid>, RoleViewModel>().ReverseMap();
        }
    }
}