using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}
