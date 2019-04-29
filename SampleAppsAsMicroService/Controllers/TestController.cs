using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace SampleAppsAsMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _httpClient = new HttpClient();

        public TestController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpClient> GetClient()
        {
            string accessToken = string.Empty;

            // get the current HttpContext to access the tokens
            var currentContext = _httpContextAccessor.HttpContext;

            var expires_at = await currentContext.GetTokenAsync("expires_at");

            if (string.IsNullOrWhiteSpace(expires_at) || DateTime.Parse(expires_at).AddSeconds(-60).ToUniversalTime() < DateTime.UtcNow)
            {
                accessToken = await RenewTokens();
                //var cp = await RenewTokens1();
            }
            else
            {

                // get access token
                accessToken = await currentContext.GetTokenAsync(
                    OpenIdConnectParameterNames.AccessToken);
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                // set as Bearer token
                _httpClient.SetBearerToken(accessToken);
            }

            _httpClient.BaseAddress = new Uri("https://localhost:44340/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }

        private async Task<string> RenewTokens()
        {
            var currentContext = _httpContextAccessor.HttpContext;
            var discoveryClient = new DiscoveryClient("https://localhost:44391/");
            var metaDataResponse = await discoveryClient.GetAsync();

            var tokenClient = new TokenClient(metaDataResponse.TokenEndpoint, "mp1", "secret");

            var currentRefreshToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
           
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentRefreshToken);
            if (!tokenResult.IsError)
            {
                var updatedToken = new List<AuthenticationToken>();
                updatedToken.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = tokenResult.IdentityToken
                });
                updatedToken.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResult.AccessToken
                });
                updatedToken.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResult.RefreshToken
                });
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                updatedToken.Add(new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                });

                var currentAuthenticationResult = await currentContext.AuthenticateAsync("Cookies");
                currentAuthenticationResult.Properties.StoreTokens(updatedToken);
                await currentContext.SignInAsync("Cookies", currentAuthenticationResult.Principal, currentAuthenticationResult.Properties);
                return tokenResult.AccessToken;
            }
            else
                throw new Exception("Error in renew=>",tokenResult.Exception);

        }

        private async Task<string> RenewTokens1()
        {
            var currentContext = _httpContextAccessor.HttpContext;

            var disco = await _httpClient.GetDiscoveryDocumentAsync("https://localhost:44391/");
            var tokenEndpoint = disco.TokenEndpoint;
            var response1 = await _httpClient.RequestTokenAsync(new TokenRequest
            {
                Address = tokenEndpoint,
                ClientId = "mp1",
                ClientSecret = "secret",
                GrantType = OpenIdConnectGrantTypes.RefreshToken,
                Parameters =
                {
        
                    { "scope", "sampleapi" }
                }

            });

            var response= await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = tokenEndpoint,
                ClientId = "mp1",
                ClientSecret = "secret",
                Scope="sampleapi",
                GrantType=OpenIdConnectGrantTypes.ClientCredentials
            });

            var tokenResult = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = tokenEndpoint,
                ClientId = "mp1",
                ClientSecret = "secret",
                RefreshToken = "refresh_token"
            });

             if (!tokenResult.IsError)
            {
                var updatedToken = new List<AuthenticationToken>();
                updatedToken.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = tokenResult.IdentityToken
                });
                updatedToken.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResult.AccessToken
                });
                updatedToken.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResult.RefreshToken
                });
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                updatedToken.Add(new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                });

                var currentAuthenticationResult = await currentContext.AuthenticateAsync("Cookies");
                currentAuthenticationResult.Properties.StoreTokens(updatedToken);
                await currentContext.SignInAsync("Cookies", currentAuthenticationResult.Principal, currentAuthenticationResult.Properties);
                return tokenResult.AccessToken;
            }
            else
                throw new Exception("Error in renew=>", tokenResult.Exception);

        }


        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await WriteOutIdentityInformation();

            // call the API
            var httpClient = await GetClient();
            var response = await httpClient.GetAsync("api/sample").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            response = await httpClient.GetAsync("api/test").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return Ok();
        }

        public async Task WriteOutIdentityInformation()
        {
            // get the saved identity token
            var identityToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            // write it out
            Debug.WriteLine($"Identity token: {identityToken}");

            // write out the user claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

    }
}
