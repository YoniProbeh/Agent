using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Server.Resources.Shared.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException() {}

        public BadRequestException(string message) : base(message) {}

        public BadRequestException(string message, Exception innerException) : base(message, innerException) {}

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}