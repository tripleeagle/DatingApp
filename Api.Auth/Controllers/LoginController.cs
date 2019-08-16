using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Services.Interfaces;
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

        [HttpGet("login/{email}/{password}")]
        public async Task<JwtWebTokenModel> SignIn(string email, string password)
        {
            return await _loginService.SignIn(email, password);
        }

        /*[Authorize]
        [HttpGet("logout/{email}")]
        public IActionResult SignOut(string email)
        {
            return _loginService.SignOut(email);
        }*/
    }
}