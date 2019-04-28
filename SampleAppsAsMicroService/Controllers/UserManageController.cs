using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
    public class UserManageController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        //[Authorize(Roles="admin,normal")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44391/");
            var iToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.SetBearerToken(accessToken);



            var useClient = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = accessToken
            });

            var claims = useClient.Claims;

            return Ok(claims);

        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
