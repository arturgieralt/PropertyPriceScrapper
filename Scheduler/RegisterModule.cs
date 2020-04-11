using Microsoft.Extensions.DependencyInjection;

namespace Scheduler
{
    public static class RegisterModule
    {
        public static void RegisterSchedulerModule(this IServiceCollection services)
        {
            // services.AddHostedService<ScrapperHostedService>();
            services.AddTransient<UrlBuilder>();
        }
    }
}