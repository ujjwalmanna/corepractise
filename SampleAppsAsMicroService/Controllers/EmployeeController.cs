using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SampleAppsAsMicroService.Models;
using SampleAppsAsMicroService.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleAppsAsMicroService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IRepository<JobRole, int> _jobRepository;
        private readonly IRepository<Employee, int> _employeeRepository;
        public EmployeeController(IRepository<JobRole,int> jobRepository, IRepository<Employee, int> employeeRepository)
        {
            _jobRepository = jobRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            WriteIdentityInfo().Wait();
            var employees = _employeeRepository.All();
            return new OkObjectResult(employees);
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var employee = _employeeRepository.Get(id);
            return new OkObjectResult(employee);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Employee employee)
        {
            using (var scope = new TransactionScope())
            {
                _employeeRepository.Add(employee);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
            }
        }

        [HttpGet]
        [Route("api/jobrole")]
        public IActionResult GetAllJobroles()
        {
            var jobRoles = _jobRepository.All();
            return new OkObjectResult(jobRoles);
        }

        public async Task WriteIdentityInfo()
        {
            var iToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            Debug.WriteLine($"identity token =>{iToken}");
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"claim type =>{claim.Type}  value =>{claim.Value}");
            }

            //var client = new HttpClient();

            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44391/");
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //client.SetBearerToken(accessToken);



            //var useClient = await client.GetUserInfoAsync(new UserInfoRequest
            //{
            //    Address = disco.UserInfoEndpoint,
            //    Token = accessToken
            //});

            //var claims = useClient.Claims;

        }
    }
}
