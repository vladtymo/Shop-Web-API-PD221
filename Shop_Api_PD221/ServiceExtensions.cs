using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi
{
    public static class ServiceExtensions
    {
        public static void AddCartService(this IServiceCollection services)
        {
            services.AddScoped<ICartService, CartService>();
            //services.AddScoped<IViewRender, ViewRender>();
        }

        public static void DisableAutoDataAnnotationValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
