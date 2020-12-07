using System;
using System.Net;

namespace BlazorApp.Api
{
    public class RequestException : Exception
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _payload;

        public RequestException(HttpStatusCode statusCode, string reason, string payload) : base(reason)
        {
            _statusCode = statusCode;
            _payload = payload;
        }

        public RequestException(HttpStatusCode statusCode, string reason, string payload, Exception innerException) : base(reason, innerException)
        {
            _statusCode = statusCode;
            _payload = payload;
        }

        public string Payload => _payload;
        public HttpStatusCode StatusCode => _statusCode;
    }
}
