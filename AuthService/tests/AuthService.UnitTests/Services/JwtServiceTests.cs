using AuthService.Application.Services;
using AuthService.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace AuthService.UnitTests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;

        public JwtServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "SuperSecretKeyForJwt"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtService = new JwtService(configuration);
        }

        [Fact]
        public void GenerateJwtToken_ShouldReturnValidToken()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser" };

            // Act
            var token = _jwtService.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.Contains(".", token); // Verifica se é um JWT válido
        }

        [Fact]
        public void GenerateJwtToken_ShouldThrowException_WhenKeyIsMissing()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>
                {
                    {"Jwt:Key", ""},
                    {"Jwt:Issuer", "TestIssuer"},
                    {"Jwt:Audience", "TestAudience"}
                };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var jwtService = new JwtService(configuration);
            var user = new User { Id = 1, Username = "testuser" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => jwtService.GenerateJwtToken(user));
        }
    }
}
