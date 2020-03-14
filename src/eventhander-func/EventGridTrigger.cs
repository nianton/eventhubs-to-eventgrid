// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EventHandler.Functions
{
    public static class EventGridTrigger
    {
        [FunctionName("EventGridTrigger")]
        public static async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var eventData = eventGridEvent.Data.ToString();
            log.LogInformation($"EventGridTrigger: event data: {eventData}");

            var response = await WebhookClient.PostAsync(eventData, log);
            if (response == null)
                log.LogInformation("EventGridTrigger: No WebhookEndpointUrl is configured");
        }
    }
}
