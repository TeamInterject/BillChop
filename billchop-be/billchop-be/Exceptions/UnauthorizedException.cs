using System;
using System.Net;

namespace BillChopBE.Exceptions
{
    public class UnauthorizedException : AbstractUserFriendlyException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public UnauthorizedException(): base() 
        {
        }

        public UnauthorizedException(string message) : base(message) 
        {
        }

        public UnauthorizedException(string message, Exception inner) : base(message, inner)
        { 
        }
    }
}
