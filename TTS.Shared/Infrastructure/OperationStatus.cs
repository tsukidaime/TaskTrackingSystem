using System.Net;

namespace TTS.Shared.Infrastructure
{
    public class OperationStatus<T>
    {
        public string Message { get; set; }
        public HttpStatusCode Code { get; set; }
        public T Value { get; set; }
    }
}