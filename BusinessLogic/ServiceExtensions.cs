using AutoMapper;
using Core.Interfaces;
using Core.Mapping;
using Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductProfile(provider.CreateScope().ServiceProvider.GetService<IFileService>()));

            }).CreateMapper());
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
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IFileService, LocalFileService>();
            services.AddScoped<IEmailSender, MailJetSender>();
        }
    }
}
