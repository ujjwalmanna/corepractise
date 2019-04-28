using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SSLAndCorsDemo.Controllers
{
   
    [ApiController]
    [Authorize]
    public class SampleController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Route("api/sample")]
        public ActionResult<IEnumerable<string>> Get()
        {
            var claims = User.Claims;
            return new string[] { "value1", "value2" };
        }

        [Route("api/test")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<string>> GetForAdmin()
        {
            return new string[] { "test1", "test2" };
        }


    }
}
