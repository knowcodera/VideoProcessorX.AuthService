using AuthService.Application.Common;
using AuthService.Domain.Interfaces;
using VideoProcessorX.Domain.Entities;
using VideoProcessorX.Domain.Interfaces;

namespace AuthService.Application.Services
{
    public class RegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventPublisher _eventPublisher;

        public RegisterService(IUserRepository userRepository, IEventPublisher eventPublisher)
        {
            _userRepository = userRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result> RegisterAsync(string username, string email, string password)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null)
                return Result.Failure("Username already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword
            };

            await _userRepository.CreateAsync(user);

            await _eventPublisher.PublishAsync("user.created", new
            {
                id = user.Id,
                email = user.Email,
                username = user.Username
            });


            return Result.Success();
        }
    }
}
