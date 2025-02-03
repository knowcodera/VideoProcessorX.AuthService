using AuthService.Presentation.Controllers;
using AuthService.Application.DTOs;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoProcessorX.Domain.Entities;
using Xunit;
using VideoProcessorX.Domain.Interfaces;

namespace AuthService.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IEventPublisher> _mockEventPublisher;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly RegisterService _registerService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockEventPublisher = new Mock<IEventPublisher>();
            _mockJwtService = new Mock<IJwtService>();
            _mockConfiguration = new Mock<IConfiguration>();

            _registerService = new RegisterService(_mockUserRepository.Object, _mockEventPublisher.Object);
            _controller = new AuthController(null, _registerService, _mockJwtService.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsSuccessfullyRegistered()
        {
            // Arrange
            var userDto = new UserRegisterDto { Username = "testuser", Email = "test@test.com", Password = "Password123!" };
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully", okResult.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUsernameAlreadyExists()
        {
            // Arrange
            var userDto = new UserRegisterDto { Username = "existingUser", Email = "test@test.com", Password = "Password123!" };
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new User { Username = "existingUser" });

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username already exists", badRequestResult.Value);
        }

        [Fact]
        public void GenerateJwtToken_ShouldReturnToken_WhenCalled()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser" };
            _mockJwtService.Setup(service => service.GenerateJwtToken(user)).Returns("valid.jwt.token");

            // Act
            var token = _mockJwtService.Object.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.Equal("valid.jwt.token", token);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var userDto = new UserLoginDto { Username = "testuser", Password = "Password123!" };
            var user = new User { Id = 1, Username = "testuser", Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password) };

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockJwtService.Setup(service => service.GenerateJwtToken(It.IsAny<User>())).Returns("valid.jwt.token");

            // Act
            var result = await _controller.Login(userDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Contains("token", result.Value.ToString());
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserDoesNotExist()
        {
            // Arrange
            var userDto = new UserLoginDto { Username = "nonexistent", Password = "Password123!" };

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(userDto) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid username or password", result.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userDto = new UserLoginDto { Username = "testuser", Password = "WrongPassword!" };
            var user = new User { Id = 1, Username = "testuser", Password = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!") };

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(userDto) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid username or password", result.Value);
        }
    }
}
