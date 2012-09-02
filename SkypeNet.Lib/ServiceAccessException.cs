using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SkypeNet.Lib
{
    public sealed class ServiceAccessException : Exception
    {
        public ServiceAccessException()
        {
        }

        public ServiceAccessException(string message) : base(message)
        {
        }

        public ServiceAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServiceAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
