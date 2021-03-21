using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using UrlShortener.FuncApp.Services;

namespace UrlShortener.FuncApp
{
    public class ShortenUrlHttpTrigger
    {
        private readonly IShortenerService _service;

        public ShortenUrlHttpTrigger(IShortenerService service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("ShortenUrlHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", "POST", Route = "shorten")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var originalUrl = "https://azure.microsoft.com";

            var result = await this._service.ShortenAsync(originalUrl);

            return new OkObjectResult(result);
        }
    }
}
