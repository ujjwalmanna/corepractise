using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAuthorizationServer.Entities;
using DemoAuthorizationServer.Models;
using DemoAuthorizationServer.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DemoAuthorizationServer.Controllers.UserRegistration
{
    public class UserRegistrationController:Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityServerInteractionService _interaction;
        public UserRegistrationController(IUserRepository userRepository, IIdentityServerInteractionService interaction)
        {
            _userRepository = userRepository;
            _interaction = interaction;
        }

        [HttpGet]
        public IActionResult RegisterUser(RegistrationInputModel registrationInputModel)
        {
            var vm = new RegisterUserViewModel
            {
                ReturnUrl = registrationInputModel.ReturnUrl,
                Provider = registrationInputModel.Provider,
                ProviderUserId = registrationInputModel.ProviderUserId
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // create user + claims
                var userToCreate = new Entities.User();
                userToCreate.Password = model.Password;
                userToCreate.Username = model.Username;
                userToCreate.IsActive = true;
                userToCreate.Claims.Add(new Entities.UserClaim("country", model.Country));
               // userToCreate.Claims.Add(new Entities.UserClaim("address", model.Address));
                userToCreate.Claims.Add(new Entities.UserClaim("given_name", model.Firstname));
                userToCreate.Claims.Add(new Entities.UserClaim("family_name", model.Lastname));
                userToCreate.Claims.Add(new Entities.UserClaim("email", model.Email));
                userToCreate.Claims.Add(new Entities.UserClaim("subscriptionlevel", "normal"));
                userToCreate.Claims.Add(new Entities.UserClaim("age", model.Age));
                if (model.IsProvisioningFromExternal)
                {
                    userToCreate.Logins.Add(new UserLogin
                    {
                        LoginProvider = model.Provider,
                        ProviderKey = model.ProviderUserId
                    });
                }

                // add it through the repository
                _userRepository.AddUser(userToCreate);

                if (!_userRepository.Save())
                {
                    throw new Exception($"Creating a user failed.");
                }

                // log the user in
                //await HttpContext.Authentication.SignInAsync(userToCreate.SubjectId, userToCreate.Username);
                if (!model.IsProvisioningFromExternal)
                {
                    await HttpContext.SignInAsync(userToCreate.SubjectId, userToCreate.Username);
                }

                // continue with the flow     
                if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            // ModelState invalid, return the view with the passed-in model
            // so changes can be made
            return View(model);
        }

    }
}
