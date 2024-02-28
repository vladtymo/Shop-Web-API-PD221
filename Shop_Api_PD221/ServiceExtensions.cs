using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

            services.AddAuthorization();
        }
    }
}
