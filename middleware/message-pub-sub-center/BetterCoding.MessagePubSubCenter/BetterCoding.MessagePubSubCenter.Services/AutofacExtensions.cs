using Autofac;
using BetterCoding.MessagePubSubCenter.Infra;
using BetterCoding.Strapi.SDK.Core;
using BetterCoding.Strapi.SDK.Core.Services;
using BetterCoding.Strapi.SDK.Core.Webhook;
using Microsoft.Extensions.Configuration;
using static BetterCoding.Strapi.SDK.Core.StrapiClient;
using BetterCoding.MessagePubSubCenter.Repository;
using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder UseServices(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterAssembly("BetterCoding.MessagePubSubCenter.Services");
            builder.UseEasyNetQ(configuration);
            builder.UseStrapiSDK(configuration);
            builder.UseElasticSearch(configuration);
            builder.EnableStrapiElasticSearch(configuration);
            return builder;
        }

        public static ContainerBuilder UseStrapiSDK(this ContainerBuilder builder, IConfiguration configuration)
        {
            var serverConfiguration = configuration.GetSection("Strapi").Get<StrapiServerConfiguration>();
            var strapiClient = new StrapiClient(serverConfiguration);

            builder.RegisterInstance(strapiClient);
            builder.RegisterInstance(strapiClient.Services).As<IServiceHub>();
            builder.RegisterInstance(strapiClient.Services.WebhookEventCoder)
                .As<IWebhookEventCoder>();
            builder.RegisterInstance(strapiClient.Services.WebhookEventClassMapping)
              .As<IWebhookEventClassMapping>();

            return builder;
        }

        public static ContainerBuilder EnableStrapiElasticSearch(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<StrapiWebhookEventElasticSearchRepository>()
                .AsImplementedInterfaces()
                .As<IElasticSearchRepository<IWebhookPayload>>()
                .InstancePerLifetimeScope();

            return builder;
        }
    }
}