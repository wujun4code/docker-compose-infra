﻿using Autofac;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection services, 
            IConfiguration configuration, 
            Action<IBusRegistrationConfigurator> configure = null)
        {
            var rabbitMQ = configuration.GetSection("RabbitMQ").Get<Entity.Configurations.RabbitMQ>();
            if (rabbitMQ == null) throw new Exception("no rebbitMq config found.");

            // doc: https://masstransit.io/quick-starts/rabbitmq
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                if (configure != null) 
                {
                    configure(x);
                }

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQ.Host, rabbitMQ.Port, rabbitMQ.Vhost, h => {
                        h.Username(rabbitMQ.Username);
                        h.Password(rabbitMQ.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}