using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BillChopBE.UserFriendlyExceptions
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
