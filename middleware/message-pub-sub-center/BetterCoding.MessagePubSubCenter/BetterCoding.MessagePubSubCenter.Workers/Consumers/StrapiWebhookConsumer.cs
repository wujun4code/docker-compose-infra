using BetterCoding.MessagePubSubCenter.Entity.Enums;
using BetterCoding.MessagePubSubCenter.Services;
using BetterCoding.Strapi.SDK.Core.Webhook;
using MassTransit;
using Newtonsoft.Json;

namespace BetterCoding.MessagePubSubCenter.Workers.Consumers
{
    public class StrapiWebhookConsumer : IConsumer<IWebhookPayload>
    {
        private readonly ILogger<StrapiWebhookConsumer> _logger;
        private readonly IStrapiWebhookService _strapiWebhookService;
        public StrapiWebhookConsumer(ILogger<StrapiWebhookConsumer> logger,
            IStrapiWebhookService strapiWebhookService)
        {
            _logger = logger;
            _strapiWebhookService = strapiWebhookService;
        }

        public async Task Consume(ConsumeContext<IWebhookPayload> context)
        {
            var json = JsonConvert.SerializeObject(context.Message);
            _logger.LogInformation("Received Json: {json}", json);
            await _strapiWebhookService.SyncToElasticSearch(context.Message);
        }
    }

    public class StrapiWebhookConsumerDefinition : ConsumerDefinition<StrapiWebhookConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<StrapiWebhookConsumer> consumerConfigurator,
            IRegistrationContext context)
        {
            var intervals = Convert(
                IntervalByMilisecond.ThreeSeconds,
                IntervalByMilisecond.FiveSeconds,
                IntervalByMilisecond.TenSeconds,
                IntervalByMilisecond.FifteenSeconds,
                IntervalByMilisecond.HalfAMinute,
                IntervalByMilisecond.OneMinute);

            endpointConfigurator.UseMessageRetry(r => r.Intervals(intervals));
        }

        int[] Convert(params IntervalByMilisecond[] values)
        {
            var array = values.Select(v => System.Convert.ToInt32(v)).ToArray();
            return array;
        }
    }
}
