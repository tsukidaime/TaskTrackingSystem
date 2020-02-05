using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTS.Shared.Infrastructure;
using TTS.Shared.Models.Role;

namespace TTS.BLL.Services.Abstract
{
    public interface IRoleService : IService
    {
        Task<OperationStatus<IEnumerable<string>>> GetByUserAsync(Guid id);
        Task<OperationStatus<T>> AssignRolesAsync<T>(T item);
        
        Task<OperationStatus<T>> RemoveRolesAsync<T>(T item);
    }
}