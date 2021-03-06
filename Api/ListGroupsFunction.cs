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
    public class ListGroupsFunction
    {
        private readonly NyaSenderClient _nyaSenderClient;
        private readonly ILogger<MirrorUserFunction> _logger;

        public ListGroupsFunction(NyaSenderClient nyaSenderClient, ILogger<MirrorUserFunction> logger)
        {
            _nyaSenderClient = nyaSenderClient;
            _logger = logger;
        }

        [FunctionName("ListGroups")]
        public async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            var principal = StaticWebAppsAuth.Parse(req);
            var adminUser = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            _logger.LogInformation($"ListGroups called by {adminUser}");

            try
            {
                var result = await _nyaSenderClient.CallListGroup();
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
