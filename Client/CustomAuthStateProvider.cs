using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using BlazorApp.Shared;
using System.Linq;

namespace BlazorApp.Client
{

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;


        public CustomAuthStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var response = await _httpClient.GetAsync("/.auth/me");
            var principal = (await response.Content.ReadFromJsonAsync<ClientPrincipalContainer>()).ClientPrincipal;

            var identity = new ClaimsIdentity(principal.IdentityProvider);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails));
            identity.AddClaims(principal.UserRoles.Select(r => new Claim(ClaimTypes.Role, r)));

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        class ClientPrincipalContainer
        {
            public ClientPrincipal ClientPrincipal { get; set; }
        }
    }
}
