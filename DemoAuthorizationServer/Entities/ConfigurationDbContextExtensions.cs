using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAuthorizationServer.Entities;
using DemoAuthorizationServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace DemoAuthorizationServer.Entities
{
    public static class ConfigurationDbContextExtensions
    {
        public static void EnsureSeedDataForContext(this ConfigurationDbContext context)
        {
            // Add 2 demo users if there aren't any users yet
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetIdentityResource())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}
