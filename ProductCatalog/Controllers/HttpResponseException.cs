using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace ProductCatalog.Controllers
{
    [Serializable]
    internal class HttpResponseException : Exception
    {
        private HttpResponseMessage msg;

        public HttpResponseException()
        {
        }

        public HttpResponseException(HttpResponseMessage msg)
        {
            this.msg = msg;
        }

        public HttpResponseException(string message) : base(message)
        {
        }

        public HttpResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}