using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Server.Resources.Shared.Exceptions
{
    public abstract class ApiException : Exception
    {
        public string Process { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public ApiException()
        {

        }

        public ApiException(string message) : base(message)
        {

        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }

    public enum Exceptions
    {
        BadRequest,
        NotFound,
        ProcessFailed,
        InvalidInput,
        Unauthorized
    }
}