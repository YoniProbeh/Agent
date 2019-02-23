using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Server.Resources.Shared.Exceptions
{
    public class NoContentException : ApiException
    {
        public NoContentException() {}

        public NoContentException(string message) : base(message) {}

        public NoContentException(string message, Exception innerException) : base(message, innerException) {}

        public NoContentException(string type, [Optional] string value) : base(message: $"No {type} Entries Were Found")
        {
            this.Type = type;
            this.Value = value;
        }

        protected NoContentException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}