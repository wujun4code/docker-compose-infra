using BetterCoding.MessagePubSubCenter.Entity;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public interface IStrapiWebhookService
    {
        Task PublishMessageAsync(StrapiWebhookPayload strapiWebhookPayload);
    }
}
