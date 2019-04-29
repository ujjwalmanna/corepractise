using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSLAndCorsDemo.Authorization
{
    public class MyPolicyHandler : AuthorizationHandler<MyPolicyRequirements>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyPolicyRequirements requirement)
        {
            var fliterContext = context.Resource as AuthorizationFilterContext;
            if(fliterContext==null)
            {
                context.Fail();
                return Task.CompletedTask;
            };
            //var data = fliterContext.RouteData.Values["id"].ToString();
            var cliam = context.User.Claims.FirstOrDefault(c => c.Value == "country").Value; ;
            if(cliam != "in")
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
