using System;
using System.Net;
using System.Runtime.Serialization;

namespace BillChopBE.Exceptions
{
    [Serializable]
    public abstract class AbstractUserFriendlyException : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }

        protected AbstractUserFriendlyException() : base()
        {
        }

        protected AbstractUserFriendlyException(string message) : base(message)
        {
        }

        protected AbstractUserFriendlyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AbstractUserFriendlyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
