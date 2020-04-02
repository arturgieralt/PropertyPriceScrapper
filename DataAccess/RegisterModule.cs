using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class RegisterModule
    {
        public static void RegisterDataAccessModule(this IServiceCollection services)
        {                
            services.AddTransient<DatabaseContext>();
            services.AddTransient<OfferRepository>();
        }       
    }
}