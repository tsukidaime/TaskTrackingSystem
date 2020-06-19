using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TTS.Shared.Infrastructure;

namespace TTS.BLL.Services.Abstract
{
    public interface IJobService : IService
    {
        Task<OperationStatus<List<T>>> GetByUser<T>(ClaimsPrincipal principal);
        Task<OperationStatus<T>> ChangeStatus<T>(T item);
    }
}