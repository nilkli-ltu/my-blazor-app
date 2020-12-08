using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http.Json;

namespace BlazorApp.Api
{
    public class NyaSenderClient 
    {
        private readonly string _baseAdress;
        private readonly HttpClient _httpClient;

        public NyaSenderClient(IConfiguration config, ILogger<NyaSenderClient> logger)
        {
            _httpClient = new HttpClient();
            _baseAdress = config.GetValue<string>("Api:BaseAdress");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config.GetValue<string>("Api:Key"));
            var trace = config.GetValue<bool>("Api:TraceEnabled", false);
            if (trace)
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
        }

        public async Task<Dictionary<string,List<string>>> CallListGroup()
        {
            var response = await _httpClient.PostAsync($"{_baseAdress}/ListGroups", new StringContent(""));
            return await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
        }

        public async Task<string> CallMirrorFunction(string user)
        {
            var response = await _httpClient.PostAsync($"{_baseAdress}/OrchestrateMirrorUser?user={HttpUtility.UrlEncode(user)}", new StringContent(""));
            var returnContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return returnContent;
            else
                throw new RequestException(response.StatusCode, response.ReasonPhrase, returnContent);
        }
    }
}
