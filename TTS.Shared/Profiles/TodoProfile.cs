using AutoMapper;
using TTS.DAL.Entities;
using TTS.Shared.Models.Todo;

namespace TTS.Shared.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Todo, TodoDto>().ReverseMap();
        }
    }
}