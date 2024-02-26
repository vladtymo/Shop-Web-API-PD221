﻿using AutoMapper;
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
        private readonly IMapper mapper;
        private readonly IValidator<RegisterModel> registerValidator;

        public AccountsService(UserManager<User> userManager, 
                                IMapper mapper, 
                                IValidator<RegisterModel> registerValidator)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.registerValidator = registerValidator;
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

        public Task Login()
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }
    }
}
