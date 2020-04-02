using BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
    public static class RegisterModule
    {
        public static void RegisterBusinessLogicModule(this IServiceCollection services)
        {
            services.AddTransient<OfferService>();
        }       
    }
}