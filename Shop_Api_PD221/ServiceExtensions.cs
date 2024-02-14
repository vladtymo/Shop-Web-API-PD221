using BusinessLogic.Interfaces;
using Shop_Api_PD221.Services;

namespace Shop_Api_PD221
{
    public static class ServiceExtensions
    {
        public static void AddCartService(this IServiceCollection services)
        {
            services.AddScoped<ICartService, CartService>();
            //services.AddScoped<IViewRender, ViewRender>();
        }
    }
}
