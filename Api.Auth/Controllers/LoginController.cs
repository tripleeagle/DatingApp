using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Controllers
{
    [ApiController]
    [Route("api/auth/login")]
    public class LoginController: ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet("signin/{email}/{password}")]
        public async Task<JwtWebTokenModel> SignIn(string email, string password)
        {
            return await _loginService.SignIn(email, password);
        }

        [Authorize]
        [HttpGet("signout/{email}")]
        public async Task SignOut(string email)
        {
            await _loginService.SignOut(email);
        }
    }
}