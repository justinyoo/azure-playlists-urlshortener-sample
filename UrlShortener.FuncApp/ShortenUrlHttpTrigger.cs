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

            var originalUrl = (string)req.Query["url"];
            using (var reader = new StreamReader(req.Body))
            {
                var serialised = await reader.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(serialised))
                {
                    var deserialised = (dynamic)JsonConvert.DeserializeObject<object>(serialised);
                    if (deserialised != null)
                    {
                        originalUrl = (string)deserialised.originalUrl;
                    }
                }
            }

            var result = await this._service.ShortenAsync(originalUrl);

            return new OkObjectResult(result);
        }
    }
}
