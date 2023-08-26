using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;
using BetterCoding.MessagePubSubCenter.Services.Pipelines.Webhook;
using BetterCoding.Patterns.Pipeline;

namespace BetterCoding.MessagePubSubCenter.Services.Pipelines
{
    public class FetchEntry : AutomicTransactionPipeline<WebhookPayloadContext>
    {
        IElasticSearchRepository _elasticSearchRepository;
        public FetchEntry(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public override async Task<WebhookPayloadContext> ProcessAsync(WebhookPayloadContext input)
        {
            var exist = input.Payload.Entry.TryGetValue("id", out var id);
            if (!exist) throw new MissingFieldException(nameof(id));
            if(id == null) throw new MissingFieldException(nameof(id));

            var validInt = int.TryParse(id.ToString(), out var idFromInt);
            if (!validInt) throw new InvalidDataException();

            Elastic.Clients.Elasticsearch.Id convertedId = idFromInt;

            var fetched = await _elasticSearchRepository.GetAsync<Dictionary<string, object>, Elastic.Clients.Elasticsearch.Id>(convertedId, input.Payload.Model);
            input.ServerFeteched = fetched;
            return input;
        }

        public override Task<WebhookPayloadContext> RevertAsync(WebhookPayloadContext input)
        {
            input.ServerFeteched = null;
            return Task.FromResult(input);
        }
    }
}
