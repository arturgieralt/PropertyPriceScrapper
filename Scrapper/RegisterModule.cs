using Microsoft.Extensions.DependencyInjection;
using Scrapper.DocumentProviders;
using Scrapper.External;
using Scrapper.Parsers;

namespace Scrapper
{
    public static class RegisterModule
    {
        public static void RegisterScrapperModule(this IServiceCollection services)
        {
            services.AddTransient<OfferParser>();
            services.AddTransient<PageCountParser>();
            services.AddTransient<HtmlWebProvider>();
            services.AddTransient<WebDocumentProvider>();
            services.AddTransient<ScrappingManager>();
        }
    }
}