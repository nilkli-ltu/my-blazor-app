using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace BlazorApp.Api
{
    public class MirrorUserFunction
    {
        private readonly NyaSenderClient _nyaSenderClient;
        private readonly ILogger<MirrorUserFunction> _logger;

        public MirrorUserFunction(NyaSenderClient nyaSenderClient, ILogger<MirrorUserFunction> logger)
        {
            _nyaSenderClient = nyaSenderClient;
            _logger = logger;
        }

        [FunctionName("MirrorUser")]
        public async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            var principal = StaticWebAppsAuth.Parse(req);
            var name = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            string user = req.Query["user"].ToString();
            if (string.IsNullOrEmpty(user)) 
                return new BadRequestObjectResult("'user' must be specified");

            try
            {
                var result = await _nyaSenderClient.CallMirrorFunction(user);
                return new OkObjectResult($"{result} by {name}");

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
