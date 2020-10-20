using System;
using System.Net;

namespace BillChopBE.Exceptions
{
    public class NotFoundException : AbstractUserFriendlyException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public NotFoundException(): base() 
        {
        }

        public NotFoundException(string message) : base(message) 
        {
        }

        public NotFoundException(string message, Exception inner) : base(message, inner)
        { 
        }
    }
}
