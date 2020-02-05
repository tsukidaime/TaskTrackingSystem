using AutoMapper;
using TTS.DAL.Entities;
using TTS.Shared.Models.Status;

namespace TTS.Web.Profiles
{
    public class JobStatusProfile : Profile
    {
        public JobStatusProfile()
        {
            CreateMap<Status, StatusDto>().ReverseMap();
        }
    }
}