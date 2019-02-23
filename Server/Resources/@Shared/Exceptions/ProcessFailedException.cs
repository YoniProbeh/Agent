using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Server.Resources.Shared.Exceptions
{
    public class ProcessFailedException : ApiException
    {
        public ProcessFailedException() {}

        public ProcessFailedException(string message) : base(message) {}

        public ProcessFailedException(string message, Exception innerException) : base(message, innerException) {}

        public ProcessFailedException(string process, string type, [Optional] string value) : base(message: String.IsNullOrEmpty(value) ? $"Failed To {process} {type}" : $"Failed To {process} {type} {value}")
        {
            this.Process = process;
            this.Type = type;
            this.Value = value;
        }
        protected ProcessFailedException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}