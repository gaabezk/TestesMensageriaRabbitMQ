using Application.Handlers;
using Domain.Services;
using Infra.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infra.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void ConnectRabbit(this IServiceCollection services)
    {
        var connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://localhost"),
            VirtualHost = "/",
            Port = 5672,
            Password = "guest",
            UserName = "guest"
        };
        
        var connection = connectionFactory.CreateConnection();
        services.AddSingleton<IConnection>(connection);
    }

    public static void AddRabbitMqService(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
    }

    public static void AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(PublishMessageToQueueCommandHandler).Assembly);
        });
    }
}