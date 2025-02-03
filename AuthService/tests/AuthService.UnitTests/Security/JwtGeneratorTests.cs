using AuthService.Infraestructure.Security;
using Microsoft.Extensions.Configuration;
using VideoProcessorX.Domain.Entities;

namespace AuthService.UnitTests.Security
{
    public class JwtGeneratorTests
    {
        private readonly JwtGenerator _jwtGenerator;

        public JwtGeneratorTests()
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

            _jwtGenerator = new JwtGenerator(configuration);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser" };

            // Act
            var token = _jwtGenerator.GenerateToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.Contains(".", token);
        }

        [Fact]
        public void GenerateToken_ShouldThrowException_WhenKeyIsMissing()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser" };

            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() => _jwtGenerator.GenerateToken(user));
        }
    }
}
