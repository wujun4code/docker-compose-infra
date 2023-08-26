using BetterCoding.MessagePubSubCenter.Repository;
using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;
using BetterCoding.Patterns.Pipeline;
using BetterCoding.Strapi.SDK.Core.Webhook;
using EasyNetQ;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public class StrapiWebhookService : IStrapiWebhookService
    {
        private readonly string _topic = "strapi.webhook";

        private readonly IBus _bus;
        private readonly IElasticSearchRepository _elasticSearchRepository;

        public StrapiWebhookService(IBus bus,
            IElasticSearchRepository elasticSearchRepository)
        {
            _bus = bus;
            _elasticSearchRepository = elasticSearchRepository;
        }

        public async Task PublishMessageAsync(WebhookPayload strapiWebhookPayload)
        {
            await _bus.PubSub.PublishAsync(strapiWebhookPayload, _topic);
        }

        public async Task SubscribeAsync(string subscriptionId, Func<WebhookPayload, CancellationToken, Task> handler)
        {
            await _bus.PubSub.SubscribeAsync(subscriptionId, handler, x => x.WithTopic(_topic));
        }

        class EntryLog
        {
            public EntryLog(WebhookPayloadContext context)
            {
                Context = context;
            }

            public WebhookPayloadContext Context { get; set; }
            public string CollectionName => $"{Context.Payload.Model}-audit-log";
        }

        class Enrty : Dictionary<string, object>
        {
            public Enrty(WebhookPayloadContext context) : base(context.Payload.Entry)
            {
                Context = context;
            }

            public WebhookPayloadContext Context { get; set; }
            public string CollectionName => Context.Payload.Model;

        }

        class WebhookPayloadContext
        {
            public WebhookPayloadContext(WebhookPayload payload)
            {
                Payload = payload;
            }

            public WebhookPayload Payload { get; set; }
        }

        class SyncEntry : AutomicTransactionPipeline<WebhookPayloadContext>
        {
            IElasticSearchRepository _elasticSearchRepository;
            public SyncEntry(IElasticSearchRepository elasticSearchRepository)
            {
                _elasticSearchRepository = elasticSearchRepository;
            }

            public override async Task<WebhookPayloadContext> ProcessAsync(WebhookPayloadContext input)
            {
                var entry = new Enrty(input);
                var serverData = entry.ToDictionary(x => x.Key, y => y.Value);
                await _elasticSearchRepository.AddAsync(serverData, entry.CollectionName);
                return input;
            }

            public override Task<WebhookPayloadContext> RevertAsync(WebhookPayloadContext input)
            {
                throw new NotImplementedException();
            }
        }

        public async Task SyncToElasticSearch(WebhookPayload webhookPayload)
        {
            var syncPipeline = new SyncEntry(_elasticSearchRepository);
            var context = new WebhookPayloadContext(webhookPayload);

            await syncPipeline.ExecuteAsync(context);
        }
    }
}
