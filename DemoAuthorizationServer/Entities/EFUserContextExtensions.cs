using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAuthorizationServer.Entities;

namespace DemoAuthorizationServer.Entities
{
    public static class EFUserContextExtensions
    {
        public static void EnsureSeedDataForContext(this EFUserContext context)
        {
            // Add 2 demo users if there aren't any users yet
            if (context.Users.Any())
            {
                return;
            }

            // init users
            var users = new List<User>()
            {
                new User
                {
                    SubjectId = "abc",//user identifier
                    Password = "password",
                    Username = "Test",
                    IsActive = true,
                    Claims = new List<UserClaim>
                    {
                        new UserClaim("given_name","Test"),
                        new UserClaim("family_name","manna"),
                        new UserClaim("email","abc@test.com"),
                        new UserClaim("age","10"),
                        new UserClaim("role","admin"),
                        new UserClaim("country","in"),
                        new UserClaim("subscriptionlevel","admin")
                    }
                },
                new User
                {
                    SubjectId = "abc-def",
                    Password = "password123",
                    Username = "Sam",
                    IsActive = true,
                    Claims = new List<UserClaim>
                    {
                        new UserClaim("given_name","Sam"),
                        new UserClaim("family_name","paul"),
                        new UserClaim("email","xyz@test.com"),
                        new UserClaim("age","20"),
                        new UserClaim("role","normal"),
                        new UserClaim("country","uk"),
                        new UserClaim("subscriptionlevel","normal")
                    }
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
