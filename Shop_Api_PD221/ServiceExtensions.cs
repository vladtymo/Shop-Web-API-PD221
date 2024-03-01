using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Services;
using WebAPI.Policies;

namespace WebApi
{
    public static class Policies
    {
        public const string PREMIUM_CLIENT = "PremiumClient";
        public const string ADULT = "Adult";
    }
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

        public static void AddJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOpts = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOpts.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.PREMIUM_CLIENT, policy =>
                    policy.RequireClaim("ClientType", ClientType.Premium.ToString()));

                options.AddPolicy(Policies.ADULT, policy =>
                    policy.AddRequirements(new MinimumAgeRequirement(18)));
            });

            services.AddAuthorizationHandles();
        }

        public static void AddAuthorizationHandles(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
        }
    }
}
