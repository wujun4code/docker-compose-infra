using BetterCoding.MessagePubSubCenter.Repository;
using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;
using BetterCoding.MessagePubSubCenter.Services.Pipelines;
using BetterCoding.MessagePubSubCenter.Services.Pipelines.Webhook;
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

        class SyncLog : AutomicTransactionPipeline<WebhookPayloadContext>
        {
            IElasticSearchRepository _elasticSearchRepository;
            public SyncLog(IElasticSearchRepository elasticSearchRepository)
            {
                _elasticSearchRepository = elasticSearchRepository;
            }

            public override async Task<WebhookPayloadContext> ProcessAsync(WebhookPayloadContext input)
            {
                var entry = new EntryLog(input);
                var serverData = entry.Context.Payload;
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
            var context = new WebhookPayloadContext(webhookPayload);
            var syncPipeline = CreatePipeline(context, _elasticSearchRepository);
            syncPipeline.Next(new SyncLog(_elasticSearchRepository));

            await syncPipeline.ExecuteAsync(context);
        }

        private AutomicTransactionPipeline<WebhookPayloadContext> DeleteWorkflow(WebhookPayloadContext input, IElasticSearchRepository elasticSearchRepository)
        {
            var fetch = new FetchEntry(elasticSearchRepository);
            var delete = new DeleteEntry(elasticSearchRepository);
            var auditLog = new AuditLog(elasticSearchRepository);
            fetch.Next(delete).Next(auditLog);

            return fetch;
        }

        private AutomicTransactionPipeline<WebhookPayloadContext> CreateWorkflow(WebhookPayloadContext input, IElasticSearchRepository elasticSearchRepository)
        {
            var create = new CreateEntry(elasticSearchRepository);
            var auditLog = new AuditLog(elasticSearchRepository);
            create.Next(auditLog);

            return create;
        }

        private AutomicTransactionPipeline<WebhookPayloadContext> CreatePipeline(WebhookPayloadContext input, IElasticSearchRepository elasticSearchRepository) => input.Payload.Event switch
        {
            null => throw new NotSupportedException(),
            "entry.create" => CreateWorkflow(input, elasticSearchRepository),
            "entry.delete" => DeleteWorkflow(input, elasticSearchRepository),
            _ => throw new NotSupportedException()
        };
    }
}
