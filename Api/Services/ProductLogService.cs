using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Domain.Entities;

namespace Api.Services
{
    public class ProductLogService : BackgroundService
    {
        private readonly IModel _channel;
        private readonly ILogger<ProductLogService> _logger;
        private readonly string _queueName;

        public ProductLogService(IConnection connection, ILogger<ProductLogService> logger)
        {
            _channel = connection.CreateModel();
            _logger = logger;
            _queueName = "product-queue";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    
                    var product = JsonConvert.DeserializeObject<Product>(message);
                    
                    if (product != null)
                    {
                        // Log do produto
                        var log = $"Received Product: Id={product.Id}, Name={product.Name}, Description={product.Description}, Category={product.Category}, PublicationDate={product.PublicationDate}";
                        _logger.LogInformation(log);
                    }
                    
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing message: {ex.Message}");
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                }
            };
            
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            
            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }
    }
}
