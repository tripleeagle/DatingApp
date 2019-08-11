using System.Threading.Tasks;
using Api.Auth.Data;
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
        
        public async Task<ActionResult> Add([FromBody] AuthCredentials authCredentials)
        {
             await _authCredentialsService.Create(authCredentials);
             return Ok();
        }
    }
}