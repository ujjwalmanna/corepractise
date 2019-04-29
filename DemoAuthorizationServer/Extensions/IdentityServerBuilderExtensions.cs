using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAuthorizationServer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DemoAuthorizationServer.Extensions
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository,UserRepository>();
            builder.AddProfileService<UserProfileService>();
            return builder;
        }
    }
}
