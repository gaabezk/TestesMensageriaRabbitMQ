using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infra.Data.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private IModel _channel;

        public RabbitMqService(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
        }
        
        public async Task PublishMessageAsync(string message, string queueName,  CancellationToken cancellationToken)
        {
            // Garante que o canal ainda está ativo
            CheckValidChanel();

            // Configura a fila antes de enviar a mensagem
            ConfigureQueue(queueName);

            var body = Encoding.UTF8.GetBytes(message);

            // Publica a mensagem de forma assíncrona
            await Task.Run(() => _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body), cancellationToken);
        }

        private void CheckValidChanel()
        {
            if (_channel is not { IsOpen: true })
                _channel = _connection.CreateModel();
        }
        
        private void ConfigureQueue(string queueName)
        {
            // Garante que a fila existe antes de enviar a mensagem
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }
    }
}
