using System;
using System.Net;

namespace BillChopBE.UserFriendlyExceptions
{
    public abstract class AbstractUserFriendlyException : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }

        public AbstractUserFriendlyException() : base()
        {
        }

        public AbstractUserFriendlyException(string message) : base(message)
        {
        }

        public AbstractUserFriendlyException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
