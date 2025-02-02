using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AuthService.Infraestructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqEventPublisher> _logger;

        public RabbitMqEventPublisher(
            IConfiguration configuration,
            ILogger<RabbitMqEventPublisher> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task PublishAsync(string queueName, object message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: false,      
                exclusive: false,
                autoDelete: false,
                arguments: null);

           
            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);

          
            await Task.CompletedTask;
        }
    }
}
