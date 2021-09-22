using System;
using System.Net;
using System.Runtime.Serialization;

namespace BillChopBE.Exceptions
{
    [Serializable]
    public class DbException : AbstractUserFriendlyException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

        public DbException() : base()
        {
        }

        public DbException(string message) : base(message)
        {
        }

        public DbException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
