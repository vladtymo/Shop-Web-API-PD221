using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
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

        public AccountsService(UserManager<User> userManager, 
                                SignInManager<User> signInManager,
                                IMapper mapper, 
                                IValidator<RegisterModel> registerValidator,
                                IJwtService jwtService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.registerValidator = registerValidator;
            this.jwtService = jwtService;
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
                Token = jwtService.CreateToken(jwtService.GetClaims(user))
            };
        }

        public async Task Logout()
        {
            //await signInManager.SignOutAsync();
        }
    }
}
