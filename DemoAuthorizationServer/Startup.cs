using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DemoAuthorizationServer.Entities;
using DemoAuthorizationServer.Extensions;
using DemoAuthorizationServer.Models;
using DemoAuthorizationServer.Services;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication.Cookies;
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

namespace DemoAuthorizationServer
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
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });
            services.AddDbContext<EFUserContext>(db => db.UseSqlServer(Configuration.GetConnectionString("IDPUserDB")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddIdentityServer()
                .AddSigningCredential(LoadCertificateFromStore()) //.AddDeveloperSigningCredential()
                .AddUserStore() // replaced by .AddTestUsers(Config.GetUsers())
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(
                        Configuration.GetConnectionString("IDPServerDB"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(
                        Configuration.GetConnectionString("IDPServerDB"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                });
            //.AddInMemoryIdentityResources(Config.GetIdentityResource())
            //.AddInMemoryApiResources(Config.GetApiResources())
            //.AddInMemoryClients(Config.GetClients());
        }

        /// <summary>
        /// Command to use create certificate:
        /// makecert -r -pe -n "CN=IDPSigningCert" -b 01/01/2019 -e 01/01/2020 -eku 1.3.6.1.5.5.7.3.3 -sky signature -a sha256 -len 2048 -ss my -sr LocalMachine
        /// </summary>
        /// <returns></returns>
        public X509Certificate2 LoadCertificateFromStore()
        {
            string thumbprint = "c62a70e681b01a65e702d27fb39af785f2ef6920";
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);
                if (certCollection.Count == 0)
                    throw new Exception("The specified certificate wasn't found");
                return certCollection[0];
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,EFUserContext userContext,ConfigurationDbContext configContext,
            PersistedGrantDbContext persistedGrantDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            configContext.Database.Migrate();
            configContext.EnsureSeedDataForContext();

            persistedGrantDbContext.Database.Migrate();

            userContext.Database.Migrate();
            userContext.EnsureSeedDataForContext();
            app.UseIdentityServer();
            
            
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();
        }
    }
}
