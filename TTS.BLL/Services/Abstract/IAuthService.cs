using System.Threading.Tasks;
using TTS.Shared.Infrastructure;

namespace TTS.BLL.Services.Abstract
{
    public interface IAuthService
    {
        Task<OperationStatus<T>> Authenticate<T>(T item);
    }
}