using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventHandler.Functions
{
    public static class WebhookClient
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static readonly string EndpointUrl = Environment.GetEnvironmentVariable("WebhookEndpointUrl");

        public static void ConfigureHttp(Action<HttpClient> configuration)
        {
            configuration?.Invoke(httpClient);
        }

        public static async Task<HttpResponseMessage> PostAsync(object payload, ILogger log)
        {
            // Handle configuration error instead
            if (string.IsNullOrWhiteSpace(EndpointUrl))
            {
                log.LogInformation("No Webhook endpoint defined, exiting...");
                return null;
            }                

            // Json-serializing object payloads
            var payloadJson = payload is string payloadString 
                ? payloadString 
                : JsonConvert.SerializeObject(payload);

            // Call the webhook
            log.LogInformation($"Post payload to Webhook endpoint '{EndpointUrl}'...");
            var httpContent = new StringContent(payloadJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(EndpointUrl, httpContent);
            return response;
        }
    }
}
