using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;
using BetterCoding.MessagePubSubCenter.Services.Pipelines.Webhook;
using BetterCoding.Patterns.Pipeline;

namespace BetterCoding.MessagePubSubCenter.Services.Pipelines
{
    public class CreateEntry: AutomicTransactionPipeline<WebhookPayloadContext>
    {
        IElasticSearchRepository _elasticSearchRepository;
        public CreateEntry(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public override async Task<WebhookPayloadContext> ProcessAsync(WebhookPayloadContext input)
        {
            var entry = new Entry(input);

            var serverData = entry.ToDictionary(x => x.Key, x => x.Value);

            await _elasticSearchRepository.AddAsync(serverData, entry.CollectionName);
            return input;
        }

        public override Task<WebhookPayloadContext> RevertAsync(WebhookPayloadContext input)
        {
            throw new NotImplementedException();
        }
    }
}
