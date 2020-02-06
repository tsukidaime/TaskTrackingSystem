using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTS.Shared.Infrastructure;

namespace TTS.BLL.Services.Abstract
{
    public interface IService
    {
        Task<OperationStatus<T>> CreateAsync<T>(T item);
        Task<OperationStatus<T>> DeleteByIdAsync<T>(Guid id);
        Task<OperationStatus<T>> UpdateAsync<T>(T item);
        Task<OperationStatus<T>> GetAsync<T>(Guid id);
        Task<OperationStatus<List<T>>> GetAllAsync<T>();
    }
}