using BetterCoding.Strapi.SDK.Core.Webhook;
using EasyNetQ;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public class StrapiWebhookService : IStrapiWebhookService
    {
        private readonly string _topic = "strapi.webhook";

        private readonly IBus _bus;

        public StrapiWebhookService(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishMessageAsync(WebhookPayload strapiWebhookPayload)
        {
            await _bus.PubSub.PublishAsync(strapiWebhookPayload, _topic);
        }

        public async Task SubscribeAsync(string subscriptionId, Func<WebhookPayload, CancellationToken, Task> handler)
        {
            await _bus.PubSub.SubscribeAsync(subscriptionId, handler, x => x.WithTopic(_topic));
        }
    }
}
