using Autofac;
using BetterCoding.MessagePubSubCenter.Infra;
using Microsoft.Extensions.Configuration;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder UseServices(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterAssembly("BetterCoding.MessagePubSubCenter.Services");
            builder.UseEasyNetQ(configuration);
            return builder;
        }
    }
}