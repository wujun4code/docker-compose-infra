using BetterCoding.MessagePubSubCenter.Entity;
using EasyNetQ;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public class StrapiWebhookService : IStrapiWebhookService
    {
        private readonly IBus _bus;

        public StrapiWebhookService(IBus bus) 
        {
            _bus = bus;
        }

        public async Task PublishMessageAsync(StrapiWebhookPayload strapiWebhookPayload)
        {
            await _bus.PubSub.PublishAsync(strapiWebhookPayload);
        }
    }
}
