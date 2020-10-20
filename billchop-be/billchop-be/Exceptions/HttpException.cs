using System;
using System.Net;

namespace BillChopBE.Exceptions
{
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
    }
}
