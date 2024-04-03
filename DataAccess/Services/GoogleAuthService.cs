using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces;
using Core.Utilities;
using Infrastructure.Data;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Infrastructure.Services
{
    internal class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ShopDbContext _context;
        private readonly GoogleAuthConfig _googleAuthConfig;

        public GoogleAuthService(
            UserManager<User> userManager,
            ShopDbContext context,
            IOptions<GoogleAuthConfig> googleAuthConfig
            )
        {
            _userManager = userManager;
            _context = context;
            _googleAuthConfig = googleAuthConfig.Value;
        }

        /// <summary>
        /// Google SignIn
        /// </summary>
        /// <param name="model">the model</param>
        /// <returns>Task&lt;BaseResponse&lt;User&gt;&gt;</returns>
        public async Task<User> GoogleSignIn(GoogleSignInDto model)
        {
            //Payload payload = new();

            //try
            //{
            //    payload = await ValidateAsync(model.IdToken, new ValidationSettings
            //    {
            //        Audience = new[] { _googleAuthConfig.ClientId }
            //    });
            //}
            //catch (Exception ex)
            //{
            //    throw new HttpException("Failed to get a response", HttpStatusCode.BadRequest);
            //}

            //var userToBeCreated = new CreateUserFromSocialLogin
            //{
            //    //FirstName = payload.GivenName,
            //    //LastName = payload.FamilyName,
            //    Email = payload.Email,
            //    //ProfilePicture = payload.Picture,
            //    LoginProviderSubject = payload.Subject,
            //};

            //var user = await _userManager.CreateUserFromSocialLogin(_context, userToBeCreated, LoginProvider.Google);

            //if (user is not null)
            //    return user;

            //else
                throw new HttpException("Unable to link a Local User to a Provider", HttpStatusCode.BadRequest);
        }
    }
}