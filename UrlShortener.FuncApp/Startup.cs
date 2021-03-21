using System;

using Azure.Data.Tables;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using UrlShortener.FuncApp.Services;

[assembly: FunctionsStartup(typeof(UrlShortener.FuncApp.Startup))]
namespace UrlShortener.FuncApp
{
    public class Startup : FunctionsStartup
    {
        private static string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IShortenerService, ShortenerService>();
            builder.Services.AddTransient<IExpanderService, ExpanderService>();

            var client = new TableServiceClient(connectionString);
            builder.Services.AddSingleton(client);
        }
    }
}
