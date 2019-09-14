using System;
using System.Threading.Tasks;
using Api.Auth.Data.Enums;
using Api.Auth.Extensions;
using Api.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        
        [AllowAnonymous]
        [HttpPost("create/{email}/{password}")]
        public async Task<ActionResult> Create (string email, string password)
        {
            Console.WriteLine("api/auth/credentials/create");
             await _authCredentialsService.Create(email, password);
             return Ok();
        }
        
        [Authorize]
        [HttpPut("change-password/{newPassword}/{email}/{password}")]
        public async Task<ActionResult> ChangePassword(string newPassword, string email, string password)
        {
            await _authCredentialsService.ChangePassword(newPassword, email, password);
            return Ok();
        }
        
        [Authorize]
        [HttpDelete("delete/{email}/{password}")]
        public async Task<ActionResult> Delete(string email, string password)
        {
            await _authCredentialsService.Delete(email, password);
            return Ok();
        }
    }
}