using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace DemoAuthorizationServer.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;
        public UserProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjId=context.Subject.GetSubjectId();
            var claimsForUser=_userRepository.GetUserClaimsBySubjectId(subjId);
            context.IssuedClaims = claimsForUser.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subjId = context.Subject.GetSubjectId();
            var isActive = _userRepository.IsUserActive(subjId);
            return Task.FromResult(0);
        }
    }
}
