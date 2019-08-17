using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Controllers
{
    [ApiController]
    [Route("api/auth/token")]
    public class TokenController: ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpGet]
        [Route("refresh/{refreshToken}/{email}")]
        private async Task<JwtWebTokenModel> Refresh(string refreshToken, string email)
        {
            return await _tokenService.Refresh(refreshToken, email);
        }    
    }
}