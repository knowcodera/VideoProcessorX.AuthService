using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}
