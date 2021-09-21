using System;
using System.Net;
using System.Runtime.Serialization;

namespace BillChopBE.Exceptions
{
    [Serializable]
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

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
