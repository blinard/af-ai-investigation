
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace af_ai_investigation
{
    public static class TestingFunction
    {
        private static string key = TelemetryConfiguration.Active.InstrumentationKey = System.Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
        // private static string aiEndpointAddy = TelemetryConfiguration.Active.TelemetryChannel.EndpointAddress = System.Environment.GetEnvironmentVariable("APPINSIGHTS_ENDPOINTADDRESS", EnvironmentVariableTarget.Process);
        private static TelemetryClient telemetry = new TelemetryClient() { InstrumentationKey = key };

        [FunctionName("TestingFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            // log.LogInformation("C# HTTP trigger function processed a request.");
            
            string name = req.Query["name"];
            log.LogInformation($"Yo-Brad: Here's da name: {name}");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
