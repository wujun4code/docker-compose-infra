using Autofac;
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
    }
}