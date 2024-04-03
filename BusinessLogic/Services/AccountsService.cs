using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Specifications;
using Core.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    internal class AccountsService : IAccountsService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;
        private readonly IValidator<RegisterModel> registerValidator;
        private readonly IJwtService jwtService;
        private readonly IRepository<RefreshToken> refreshTokenR;
        private readonly IGoogleAuthService googleAuthService;

        public AccountsService(UserManager<User> userManager, 
                                SignInManager<User> signInManager,
                                IMapper mapper, 
                                IValidator<RegisterModel> registerValidator,
                                IJwtService jwtService,
                                IRepository<RefreshToken> refreshTokenR,
                                IGoogleAuthService googleAuthService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.registerValidator = registerValidator;
            this.jwtService = jwtService;
            this.refreshTokenR = refreshTokenR;
            this.googleAuthService = googleAuthService;
        }

        public async Task Register(RegisterModel model)
        {
            registerValidator.ValidateAndThrow(model);

            if (await userManager.FindByEmailAsync(model.Email) != null)
                throw new HttpException("Email is already exists.", HttpStatusCode.BadRequest);

            var user = mapper.Map<User>(model);

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new HttpException(string.Join(" ", result.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);
        }

        public async Task<LoginResponseDto> Login(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                throw new HttpException("Invalid login or password.", HttpStatusCode.BadRequest);

            //await signInManager.SignInAsync(user, true);

            return new LoginResponseDto()
            {
                AccessToken = jwtService.CreateToken(jwtService.GetClaims(user)),
                RefreshToken = CreateRefreshToken(user.Id).Token
            };
        }

        private RefreshToken CreateRefreshToken(string userId)
        {
            var refeshToken = jwtService.CreateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refeshToken,
                UserId = userId,
                CreationDate = DateTime.UtcNow // Now vs UtcNow
            };

            refreshTokenR.Insert(refreshTokenEntity);
            refreshTokenR.Save();

            return refreshTokenEntity;
        }

        public async Task Logout(string refreshToken)
        {
            //await signInManager.SignOutAsync();

            var refrestTokenEntity = await refreshTokenR.GetItemBySpec(new RefreshTokenSpecs.ByToken(refreshToken));

            if (refrestTokenEntity == null)
                throw new HttpException(Errors.InvalidToken, HttpStatusCode.BadRequest);

            refreshTokenR.Delete(refrestTokenEntity);
            refreshTokenR.Save();
        }

        public async Task<UserTokens> RefreshTokens(UserTokens userTokens)
        {
            var refreshToken = await refreshTokenR.GetItemBySpec(new RefreshTokenSpecs.ByToken(userTokens.RefreshToken));

            if (refreshToken == null || refreshToken.CreationDate < jwtService.GetLastValidRefreshTokenDate())
                throw new HttpException(Errors.InvalidToken, HttpStatusCode.BadRequest);

            var claims = jwtService.GetClaimsFromExpiredToken(userTokens.AccessToken);
            var newAccessToken = jwtService.CreateToken(claims);
            var newRefreshToken = jwtService.CreateRefreshToken();

            refreshToken.Token = newRefreshToken;

            refreshTokenR.Update(refreshToken);
            refreshTokenR.Save();

            var tokens = new UserTokens()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return tokens;
        }

        public async Task RemoveExpiredRefreshTokens()
        {
            var lastDate = jwtService.GetLastValidRefreshTokenDate();
            var expiredTokens = await refreshTokenR.GetListBySpec(new RefreshTokenSpecs.ByDate(lastDate));

            foreach (var i in expiredTokens)
            {
                refreshTokenR.Delete(i);
            }
            refreshTokenR.Save();
        }

        /// <summary>
        /// Google SignIn 
        /// </summary>
        /// <param name="model">the view model</param>
        /// <returns>Task&lt;BaseResponse&lt;JwtResponseVM&gt;&gt;</returns>
        public async Task<LoginResponseDto> SignInWithGoogle(GoogleSignInDto model)
        {
            var user = await googleAuthService.GoogleSignIn(model);

            // throw HttpException

            return new LoginResponseDto()
            {
                AccessToken = jwtService.CreateToken(jwtService.GetClaims(user)),
                RefreshToken = CreateRefreshToken(user.Id).Token
            };
        }
    }
}
