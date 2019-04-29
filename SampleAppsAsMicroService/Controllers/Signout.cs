using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleAppsAsMicroService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class Signout : Controller
    {
        private HttpClient _httpClient = new HttpClient();
        // GET: api/<controller>
        [HttpGet]
        public async Task LogOut()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44391/");
            var metaDataResponse = await discoveryClient.GetAsync();
            var revocationClient = new TokenRevocationClient(metaDataResponse.RevocationEndpoint, "mp1", "secret");
            var accessToken=await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var revokeTokenResponse = await revocationClient.RevokeAccessTokenAsync(accessToken);
                if (revokeTokenResponse.IsError)
                    throw new Exception("access error", revokeTokenResponse.Exception);
            }

            //var disco = await _httpClient.GetDiscoveryDocumentAsync("https://localhost:44391/");
            //var revocationClient1 = await _httpClient.RevokeTokenAsync(new TokenRevocationRequest
            //{
            //    Address = disco.RegistrationEndpoint,
            //    ClientId = "mp1",
            //    ClientSecret = "secret"

            //});
            //var accessToken1 = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

       
    }
}
