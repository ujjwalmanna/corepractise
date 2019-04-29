using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSLAndCorsDemo.Authorization
{
    public class MyPolicyRequirements:IAuthorizationRequirement
    {
        public MyPolicyRequirements()
        {

        }
    }
}
