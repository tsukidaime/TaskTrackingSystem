using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTS.Shared.Infrastructure;

namespace TTS.BLL.Services.Abstract
{
    public interface ITodoService : IService
    {
        Task<OperationStatus<List<T>>> GetByJobAsync<T>(Guid id);
    }
}