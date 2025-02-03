using AuthService.Infraestructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuthService.UnitTests.Messaging
{
    public class RabbitMqEventPublisherTests
    {
        private readonly Mock<ILogger<RabbitMqEventPublisher>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly RabbitMqEventPublisher _publisher;

        public RabbitMqEventPublisherTests()
        {
            _mockLogger = new Mock<ILogger<RabbitMqEventPublisher>>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(x => x["RabbitMQ:HostName"]).Returns("localhost");
            _mockConfiguration.Setup(x => x["RabbitMQ:Username"]).Returns("guest");
            _mockConfiguration.Setup(x => x["RabbitMQ:Password"]).Returns("guest");

            _publisher = new RabbitMqEventPublisher(_mockConfiguration.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task PublishAsync_ShouldLogMessage_WhenCalled()
        {
            // Arrange
            var queueName = "testQueue";
            var message = new { Id = 1, Name = "Test Message" };

            // Act
            await _publisher.PublishAsync(queueName, message);

            // Assert
            _mockLogger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Published message")),
                It.IsAny<System.Exception>(),
                It.IsAny<System.Func<It.IsAnyType, System.Exception, string>>()),
                Times.Once);
        }
    }
}
