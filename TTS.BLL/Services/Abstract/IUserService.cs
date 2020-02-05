using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTS.DAL.Entities;
using TTS.Shared.Infrastructure;

namespace TTS.BLL.Services.Abstract
{
    public interface IUserService : IService
    {
        Task<OperationStatus<IEnumerable<T>>> GetByJobAsync<T>(Job job);
    }
}