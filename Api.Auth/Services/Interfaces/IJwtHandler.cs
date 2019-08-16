using Api.Auth.Data;
using Microsoft.IdentityModel.Tokens;

namespace Api.Auth.Services.Interfaces
{
    public interface IJwtHandler
    {
        SymmetricSecurityKey GetSymmetricSecurityKey();
        JwtWebTokenModel Generate(string email);
    }
}