using AutoMapper;
using TTS.DAL.Entities;
using TTS.Shared.Models.Job;

namespace TTS.Shared.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobDto>()
                .ForMember(dest=>dest.Status,
                    opt=>opt.MapFrom(x=>x.Status))
                .ReverseMap();
            CreateMap<Job, JobDetailsDto>().ReverseMap();
            CreateMap<Job, JobCreateDto>().ReverseMap();
            CreateMap<Job, JobEditDto>().ReverseMap();
        }
    }
}