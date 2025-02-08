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

        public RabbitMqEventPublisher(IConfiguration configuration, ILogger<RabbitMqEventPublisher> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task PublishAsync(string routingKey, object message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"], 
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"],
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: "user_exchange",
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false
            );

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var props = channel.CreateBasicProperties();
            props.Persistent = true;

            channel.BasicPublish(
                exchange: "user_exchange",
                routingKey: routingKey, 
                basicProperties: props,
                body: body
            );

            _logger.LogInformation("Published event to user_exchange, routingKey={rk}, body={body}", routingKey, json);
            await Task.CompletedTask;
        }
    }
}
