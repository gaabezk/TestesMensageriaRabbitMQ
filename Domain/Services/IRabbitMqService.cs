using Domain.Entities;

namespace Domain.Services
{
    public interface IRabbitMqService
    {
        Task PublishMessageAsync(string message, string queueName, CancellationToken cancellationToken);
    }
}