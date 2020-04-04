using Microsoft.Extensions.DependencyInjection;

namespace MailService
{
    public static class RegisterModule
    {
        public static void RegisterMailServiceModule(this IServiceCollection services)
        {                
            services.AddTransient<MailSender>();
        }       
    }
}