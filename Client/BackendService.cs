using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace BlazorApp.Client
{
    public class BackendService
    {
        private readonly HttpClient _httpClient;

        public BackendService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CallMirrorUser(string username)
        {
            return await _httpClient.GetStringAsync($"/api/MirrorUser?user={username}");
        }

        public async Task<Dictionary<string, List<string>>> CallListGroups()
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<string, List<string>>>("/api/ListGroups");
        }
      
    }
}
