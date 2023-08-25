using Autofac;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace BetterCoding.MessagePubSubCenter.Infra
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterAssembly(this ContainerBuilder builder, string assemblyString)
        {
            builder.RegisterAssemblyTypes(Assembly.Load(assemblyString))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return builder;
        }

        public static ContainerBuilder UseEasyNetQ(this ContainerBuilder builder, IConfiguration configuration)
        {
            var rebbitMqUrl = configuration.GetConnectionString("RabbitMq");
            if (rebbitMqUrl == null)
            {
                throw new Exception("no specific rebbitMq url found.");
            }
            var bus = RabbitHutch.CreateBus(rebbitMqUrl, register => register.EnableConsoleLogger());

            builder.RegisterInstance(bus).As<IBus>().SingleInstance();

            return builder;
        }
    }
}