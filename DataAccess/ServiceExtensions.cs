using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceExtensions
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShopDbContext>(opts =>
                opts.UseSqlServer(connectionString));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
               //.AddDefaultTokenProviders()
               .AddEntityFrameworkStores<ShopDbContext>();
        }

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        }
    }
}
