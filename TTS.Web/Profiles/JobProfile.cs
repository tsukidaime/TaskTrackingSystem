using AutoMapper;
using TTS.DAL.Entities;
using TTS.Shared.Models.Job;

namespace TTS.Web.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobModel>().ReverseMap();
            CreateMap<Job, JobDetailsModel>().ReverseMap();
        }
    }
}