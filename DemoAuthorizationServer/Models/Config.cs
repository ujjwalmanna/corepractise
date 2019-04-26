﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace DemoAuthorizationServer.Models
{

    //iss=>issuer of token aud=>client name amr=>authentication mode nonce=>protect against cross side forgery
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "abc",//user identifier
                    Password = "password",
                    Username = "Test",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name","Test"),
                        new Claim("family_name","manna"),
                        new Claim("email","abc@test.com"),
                        new Claim("age","10")
                    }
                },
                new TestUser
                {
                    SubjectId = "abc-def",
                    Password = "password123",
                    Username = "Sam",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name","Sam"),
                        new Claim("family_name","paul"),
                        new Claim("email","xyz@test.com"),
                        new Claim("age","20")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new Age()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "MP Trader",
                    ClientId = "mp1",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44369/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                         "https://localhost:44369/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "age"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                   // AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }

    public class Age : IdentityResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:IdentityServer4.Models.IdentityResources.Age" /> class.
        /// </summary>
        public Age()
        {
            this.Name = "age";
            this.DisplayName = "Your age";
            this.Emphasize = true;
            this.UserClaims = (ICollection<string>)new string[1] {"age"}.ToList<string>();
        }
    }
}