using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using BlazorApp.Shared;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.Client
{

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private bool _testMode;
        private List<string> _testRoles;

        public CustomAuthStateProvider(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _testMode = config.GetValue<bool>("TestMode", false);

            if (_testMode)
            {
                _testRoles= config.GetValue<string>("TestRoles", "").Split(",").ToList();
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClientPrincipal principal = _testMode ? await GetTestUserPrincipal() : await GetUserPrincipal();

            var identity = new ClaimsIdentity(principal.IdentityProvider);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails));
            identity.AddClaims(principal.UserRoles.Select(r => new Claim(ClaimTypes.Role, r)));

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        private async Task<ClientPrincipal> GetUserPrincipal()
        {
            var response = await _httpClient.GetAsync("/.auth/me");
            var principal = (await response.Content.ReadFromJsonAsync<ClientPrincipalContainer>()).ClientPrincipal;
            return principal;
        }

        private Task<ClientPrincipal> GetTestUserPrincipal()
        {
            return Task.FromResult(new ClientPrincipal
            {
                IdentityProvider = "Test",
                UserDetails = "test@testson",
                UserId = "1234567",
                UserRoles = _testRoles
            }); ;
        }
        class ClientPrincipalContainer
        {
            public ClientPrincipal ClientPrincipal { get; set; }
        }
    }
}
