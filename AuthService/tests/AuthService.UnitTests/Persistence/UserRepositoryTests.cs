using AuthService.Domain.Entities;
using AuthService.Infraestructure.Data;
using AuthService.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.UnitTests.Persistence
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _repository;
        private readonly AppDbContext _dbContext;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            _dbContext = new AppDbContext(options);
            _repository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User { Username = "testuser", Email = "test@test.com", Password = "hashedpass" };

            // Act
            await _repository.CreateAsync(user);
            var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == "testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Username = "existinguser", Email = "existing@test.com", Password = "hashedpass" };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUsernameAsync("existinguser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("existinguser", result.Username);
        }
    }
}
