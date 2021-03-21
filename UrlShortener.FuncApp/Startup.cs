using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using UrlShortener.FuncApp.Services;

[assembly: FunctionsStartup(typeof(UrlShortener.FuncApp.Startup))]
namespace UrlShortener.FuncApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IShortenerService, ShortenerService>();
        }
    }
}
