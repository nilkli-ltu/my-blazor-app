using BlazorApp.Shared;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

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
            var response = await _httpClient.GetAsync($"/api/MirrorUser?user={username}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await response.Content.ReadAsStringAsync();
                case HttpStatusCode.NotFound:
                    throw new Exception("Användaren hittades ej");
                default:
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<ClientPrincipal> GetUserInfo()
        {
            var response = await _httpClient.GetAsync("/.auth/me");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return (await response.Content.ReadFromJsonAsync<ClientPrincipalContainer>()).ClientPrincipal;
                case HttpStatusCode.NotFound:
                    return null;
                default:
                    throw new Exception(response.ReasonPhrase);
            }
        }
        
        class ClientPrincipalContainer
        {
            public ClientPrincipal ClientPrincipal { get; set; }
        }
    }
}
