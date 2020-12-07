using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;


namespace BlazorApp.Api
{
    public class MirrorUserFunction
    {
        private readonly NyaSenderClient _nyaSenderRestClient;
        private readonly ILogger<MirrorUserFunction> _logger;

        public MirrorUserFunction(NyaSenderClient nyaSenderRestClient, ILogger<MirrorUserFunction> logger)
        {
            _nyaSenderRestClient = nyaSenderRestClient;
            _logger = logger;
        }

        [FunctionName("MirrorUser")]
        public async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            string user = req.Query["user"].ToString();
            if (string.IsNullOrEmpty(user)) 
                return new BadRequestObjectResult("'user' must be specified");

            try
            {
                var result = await _nyaSenderRestClient.CallMirrorFunction(user);
                return new OkObjectResult(result);

            }catch(RequestException e)
            {
                return new ObjectResult(e.Payload ?? e.Message)
                {
                    StatusCode = (int)e.StatusCode
                };
            }
        }
    }
}
