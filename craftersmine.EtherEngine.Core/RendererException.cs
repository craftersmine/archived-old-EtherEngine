using System;
using System.Runtime.Serialization;

namespace craftersmine.EtherEngine.Core
{
    [Serializable]
    internal class RendererException : Exception
    {
        public RendererException()
        {
        }

        public RendererException(string message) : base(message)
        {
        }

        public RendererException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RendererException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}