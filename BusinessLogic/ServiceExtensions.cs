using AutoMapper;
using Core.Interfaces;
using Core.Mapping;
using Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core
{
    public static class ServiceExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                var fileService = provider.CreateScope().ServiceProvider.GetService<IFileService>()!;
                var http = provider.CreateScope().ServiceProvider.GetService<IHttpContextAccessor>()!;

                var Request = http.HttpContext.Request;

                var builder = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value);
                string host = builder.ToString(); //http.HttpContext!.Request.Host.ToString();

                cfg.AddProfile(new ProductProfile(fileService, host));
                cfg.AddProfile(new AccountProfile());

            }).CreateMapper());

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddAutoMapper((serviceProvider, mapperConfiguration) =>
            //{
            //    var request = serviceProvider?.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Request;
            //    var fileService = serviceProvider?.GetRequiredService<IFileService>();
            //    var urlStr = new Uri($"{request?.Scheme}://{request?.Host.Host}:{request?.Host.Port}/").AbsoluteUri;

            //    //mapperConfiguration.AddProfile(new AccountProfile());
            //    mapperConfiguration.AddProfile(new ProductProfile(fileService, urlStr));
            //});
        }

        public static void AddFluentValidator(this IServiceCollection services)
        {
            //services.AddFluentValidationAutoValidation();
            // enable client-side validation
            //services.AddFluentValidationClientsideAdapters();
            // Load an assembly reference rather than using a marker type.
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IFileService, AzureBlobService/*LocalFileService*/>();
            services.AddScoped<IEmailSender, MailJetSender>();
        }
    }
}
