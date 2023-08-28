using Autofac;
using Autofac.Extensions.DependencyInjection;
using BetterCoding.MessagePubSubCenter.API.Middlewares;
using BetterCoding.MessagePubSubCenter.Infra;
using BetterCoding.MessagePubSubCenter.Services;
using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureServices((hostContext, services) =>
{
    services.AddMassTransitRabbitMq(builder.Configuration, x =>
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        x.AddConsumers(entryAssembly);
        x.AddSagaStateMachines(entryAssembly);
        x.AddSagas(entryAssembly);
        x.AddActivities(entryAssembly);
    });
});

builder.Host.ConfigureContainer<ContainerBuilder>(autofac =>
{
    autofac.UseServices(builder.Configuration);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAPIKeyMiddleware(builder.Configuration);
app.MapControllers();

app.Run();
