using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoProcessorX.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}
