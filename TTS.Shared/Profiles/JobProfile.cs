using AutoMapper;
using TTS.DAL.Entities;
using TTS.Shared.Models.Job;

namespace TTS.Web.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<Job, JobDetailsDto>().ReverseMap();
        }
    }
}