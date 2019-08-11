using System;
using System.Threading.Tasks;
using Api.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Controllers
{
    [Route("api/auth/credentials")]
    [ApiController]
    public class AuthCredentialsController: ControllerBase
    {
        private readonly IAuthCredentialsService _authCredentialsService;
        public AuthCredentialsController(IAuthCredentialsService authCredentialsService)
        {
            _authCredentialsService = authCredentialsService;
        }
        
        [HttpGet("create/{email}/{password}")]
        public async Task<ActionResult> Create (string email, string password)
        {
            Console.WriteLine("api/auth/credentials/create");
             await _authCredentialsService.Create(email, password);
             return Ok();
        }
        
        [HttpGet("change-password/{newPassword}/{email}/{password}")]
        public async Task<ActionResult> ChangePassword([FromBody] string newPassword, string email, string password)
        {
            await _authCredentialsService.ChangePassword(newPassword, email, password);
            return Ok();
        }
        
        [HttpGet("delete/{email}/{password}")]
        public async Task<ActionResult> Delete([FromBody] string email, string password)
        {
            await _authCredentialsService.Delete(email, password);
            return Ok();
        }
    }
}