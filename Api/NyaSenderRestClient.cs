using Ltu.Int.Common.Rest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace BlazorApp.Api
{
    public class NyaSenderRestClient : AbstractRestClient
    {
        private readonly string _apiKey;
        private readonly bool _trace;
        public NyaSenderRestClient(IConfiguration config, ILogger<IRestClient>logger) : base(config.GetValue<string>("Api:BaseAdress"), logger)
        {
            _apiKey = config.GetValue<string>("Api:Key");
            _trace = config.GetValue<bool>("Api:TraceEnabled", false);
        }

        protected override void CustomizeRequest(HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _apiKey);
            if(_trace)
                requestMessage.Headers.Add("Ocp-Apim-Trace", "true");
        }

        protected override string GetDefaultMediaType()
        {
            return "text/plain";
        }
    }
}
