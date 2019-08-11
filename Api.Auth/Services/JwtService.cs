using System.Text;
using Api.Auth.Models;
using Api.Auth.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Auth.Services
{
    public class JwtService: IJwtService
    {
        private readonly AuthConfig _authConfig;
        
        public JwtService(IOptions<AuthConfig> authConfig)
        {
            _authConfig = authConfig.Value;
        }
        
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authConfig.Key));
        }
    }
}