using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using UrlShortener.FuncApp.Services;

namespace UrlShortener.FuncApp
{
    public class ExpandUrlHttpTrigger
    {
        private readonly IExpanderService _service;

        public ExpandUrlHttpTrigger(IExpanderService service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [FunctionName("ExpandUrlHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "expand")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var shortUrl = (string)req.Query["url"];

            var result = await this._service.ExpandAsync(shortUrl);

            return string.IsNullOrWhiteSpace(result.OriginalUrl)
                ? (IActionResult) new NotFoundResult()
                : new RedirectResult(result.OriginalUrl);
        }
    }
}
