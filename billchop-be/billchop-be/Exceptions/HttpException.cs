using System;
using System.Net;
using System.Runtime.Serialization;

namespace BillChopBE.Exceptions
{
    [Serializable]
    public class HttpException : AbstractUserFriendlyException
    {
        public override HttpStatusCode StatusCode { get; }

        public HttpException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = statusCode;
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
