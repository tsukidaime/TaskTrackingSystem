using AutoMapper;
using TTS.DAL.Entities;
using TTS.Shared.Models.Status;

namespace TTS.Shared.Profiles
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<Status, StatusDto>().ReverseMap();
        }
    }
}