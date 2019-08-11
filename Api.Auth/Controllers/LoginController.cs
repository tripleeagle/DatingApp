using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Api.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Controllers
{
    [Route("api/auth/login")]
    [ApiController]
    public class LoginController: ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet("login/{email}/{password}")]
        public async Task<IActionResult> Login(string email, string password)
        {
            return await _loginService.Login(email, password);
        }

        [HttpGet("logout/{email}")]
        public IActionResult Logout(string email)
        {
            return _loginService.Logout(email);
        }

        [HttpPost("refresh-token/{email}")]
        public IActionResult Refresh(string email, JwtSecurityToken jwtSecurityToken)
        {
            return _loginService.Refresh(email, jwtSecurityToken);
        }
    }
}