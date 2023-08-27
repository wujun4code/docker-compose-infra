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


        public async Task SyncToElasticSearch(WebhookPayload webhookPayload)
        {
            var context = new WebhookPayloadContext(webhookPayload);

            var syncPipeline = CreatePipeline(context, _elasticSearchRepository);

            await syncPipeline.ExecuteAsync(context);
        }

        private AutomicTransactionPipeline<WebhookPayloadContext> CreateWorkflow(WebhookPayloadContext input, IElasticSearchRepository elasticSearchRepository)
        {
            var create = new CreateEntry(elasticSearchRepository);
            var auditLog = new AuditLog(elasticSearchRepository);
            create.Next(auditLog);

            return create;
        }

        private AutomicTransactionPipeline<WebhookPayloadContext> EditWorkflow(WebhookPayloadContext input, IElasticSearchRepository elasticSearchRepository)
        {
            var fetch = new FetchEntry(elasticSearchRepository);
            var edit = new EditEntry(elasticSearchRepository);
            var auditLog = new AuditLog(elasticSearchRepository);

            fetch.Next(edit).Next(auditLog);
            return fetch;
        }

        private AutomicTransactionPipeline<WebhookPayloadContext> DeleteWorkflow(WebhookPayloadContext input, IElasticSearchRepository elasticSearchRepository)
        {
            var fetch = new FetchEntry(elasticSearchRepository);
            var delete = new DeleteEntry(elasticSearchRepository);
            var auditLog = new AuditLog(elasticSearchRepository);
            fetch.Next(delete).Next(auditLog);

            return fetch;
        }



        private AutomicTransactionPipeline<WebhookPayloadContext> CreatePipeline(WebhookPayloadContext input, IElasticSearchRepository elasticSearchRepository) => input.Payload.Event switch
        {
            null => throw new NotSupportedException(),
            "entry.create" => CreateWorkflow(input, elasticSearchRepository),
            "entry.delete" => DeleteWorkflow(input, elasticSearchRepository),
            "entry.update" => EditWorkflow(input, elasticSearchRepository),
            _ => throw new NotSupportedException()
        };
    }
}
