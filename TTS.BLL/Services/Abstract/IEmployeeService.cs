using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTS.Shared.Infrastructure;

namespace TTS.BLL.Services.Abstract
{
    public interface IEmployeeService
    {
        Task<OperationStatus<IEnumerable<T>>> GetAsync<T>(Guid id);
        
        Task<OperationStatus<T>> CreateAsync<T>(T item);
    }
}