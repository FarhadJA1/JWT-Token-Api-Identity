using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_ApiIdentity.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username, List<string> roles);
    }
}
