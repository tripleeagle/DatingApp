using Microsoft.IdentityModel.Tokens;

namespace Api.Auth.Services.Interfaces
{
    public interface IJwtService
    {
        SymmetricSecurityKey GetSymmetricSecurityKey();
    }
}