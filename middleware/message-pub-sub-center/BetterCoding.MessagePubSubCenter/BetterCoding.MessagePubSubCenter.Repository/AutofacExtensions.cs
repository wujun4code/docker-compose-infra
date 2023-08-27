using Autofac;
using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;

namespace BetterCoding.MessagePubSubCenter.Repository
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder UseElasticSearch(this ContainerBuilder builder, IConfiguration configuration)
        {
            var elasticsearchUrl = configuration.GetConnectionString("ElasticSearch");
            if (elasticsearchUrl == null)
            {
                throw new Exception("no specific elasticsearch url found.");
            }

            var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUrl))
#if DEBUG
                .EnableDebugMode()
#endif
                .PrettyJson()
                .RequestTimeout(TimeSpan.FromMinutes(2));

            var client = new ElasticsearchClient(settings);

            builder.RegisterInstance(client).As<ElasticsearchClient>().SingleInstance();

            builder.RegisterType<EasyElasticSearchRepository>()
            .As<ElasticSearchRepository>()
            .AsImplementedInterfaces().InstancePerLifetimeScope();

             builder.RegisterType<EasyElasticSearchRepository>()
            .As<IElasticSearchRepository>()
            .AsImplementedInterfaces().InstancePerLifetimeScope();
            
            builder.RegisterType<RepositoryHub>()
            .As<IRepositoryHub>()
            .AsImplementedInterfaces().InstancePerLifetimeScope();
            return builder;
        }
    }
}