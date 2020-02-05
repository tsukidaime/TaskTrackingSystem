using System.Net;

namespace TTS.Shared.Infrastructure
{
    public class OperationHelper
    {
        public OperationStatus<T> OK<T>(T value, string message)
        {
            return new OperationStatus<T>()
            {
                Value = value,
                Message = message,
                Code = HttpStatusCode.OK
            };
        }
        
        public OperationStatus<T> NotFound<T>(string message)
        {
            return new OperationStatus<T>()
            {
                Message = message,
                Code = HttpStatusCode.NotFound
            };
        }
        
        public OperationStatus<T> OK<T>(string message)
        {
            return new OperationStatus<T>()
            {
                Message = message,
                Code = HttpStatusCode.OK
            };
        }

        public OperationStatus<T> InternalServerError<T>(string message)
        {
            return new OperationStatus<T>()
            {
                Message = message,
                Code = HttpStatusCode.InternalServerError
            };
        }

        public OperationStatus<T> BadRequest<T>(string message)
        {
            return new OperationStatus<T>()
            {
                Message = message,
                Code = HttpStatusCode.BadRequest
            };
        }
    }
}