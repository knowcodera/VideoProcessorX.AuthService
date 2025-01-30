using AuthService.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace AuthService.Infraestructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly string _hostName;

        public RabbitMqEventPublisher(string hostName)
        {
            _hostName = hostName;
        }

        public async Task PublishAsync(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "UserEvents", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "UserEvents", basicProperties: null, body: body);

            await Task.CompletedTask;
        }
    }
}
