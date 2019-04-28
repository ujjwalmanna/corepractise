using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SampleAppsAsMicroService.Efs;
using SampleAppsAsMicroService.Models;
using SampleAppsAsMicroService.Repositories;

namespace SampleAppsAsMicroService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<EfContext>(db => db.UseSqlServer(Configuration.GetConnectionString("PlaygroundDB")));
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            // services.AddTransient<IRepository<JobRole,int>>(s => new Repository<JobRole,int>());
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", (options) =>
                {
                    options.AccessDeniedPath = "/authorization/accessdenied";
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.Authority = "https://localhost:44391/";
                    options.SignInScheme = "Cookies";
                    options.ClientId = "mp1";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("age");
                    options.Scope.Add("roles");
                    options.Scope.Add("sampleapi");
                    options.ResponseType = "code id_token";
                    options.SaveTokens = true;
                    options.ClientSecret = "secret";
                    
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClaimActions.MapUniqueJsonKey("role", "role");
                    options.ClaimActions.Remove("amr");
                    options.ClaimActions.DeleteClaim("sid");
                    options.ClaimActions.DeleteClaim("idp");
                    options.ClaimActions.DeleteClaim("email");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.GivenName,
                        RoleClaimType = JwtClaimTypes.Role,
                    };


                    //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    //{
                    //    RoleClaimType = JwtClaimTypes.Role,
                    //    NameClaimType = JwtClaimTypes.GivenName
                    //};
                    //options.Events = new OpenIdConnectEvents()
                    //{
                    //    OnTokenValidated = tokenValidatedContext =>
                    //    {
                    //        var identity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;
                    //        var subClaims = identity.Claims.FirstOrDefault(c => c.Type == "sub");
                    //        var newClaimIdentity=new ClaimsIdentity(tokenValidatedContext.Scheme.Name,"given_name","role");
                    //        newClaimIdentity.AddClaim(subClaims);
                    //        tokenValidatedContext = new TokenValidatedContext(tokenValidatedContext.HttpContext,
                    //            tokenValidatedContext.Scheme,
                    //            tokenValidatedContext.Options,
                    //            new ClaimsPrincipal(newClaimIdentity),
                    //            tokenValidatedContext.Properties);


                    //        return Task.FromResult(0);
                    //    },
                    //    OnUserInformationReceived = userInformationReceivedContext=>
                    //    {
                    //        userInformationReceivedContext.User.Remove("email");
                    //        //userInformationReceivedContext.User.Remove("age");
                    //        return Task.FromResult(0);
                    //    }
                    //};

                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //    .AddCookie("cookies", options =>
            //{
            //    options.ForwardDefaultSelector = ctx =>
            //        ctx.Request.Path.StartsWithSegments("/api") ? "jwt" : "cookies";
            //})
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
            
        }
    }
}
