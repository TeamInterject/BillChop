﻿using System;
using System.Net;
using System.Runtime.Serialization;

namespace BillChopBE.Exceptions
{
    [Serializable]
    public class BadRequestException : AbstractUserFriendlyException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public BadRequestException() : base()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
