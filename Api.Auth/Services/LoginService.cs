using System;
using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Models.Exceptions;
using Api.Auth.Services.Interfaces;

namespace Api.Auth.Services
{
    public class LoginService: ILoginService
    {
        private readonly RepositoryContext _db;
        private readonly ITokenService _tokenService;
        private readonly IAuthCredentialsService _authCredentialsService;
        private readonly IJwtHandler _jwtHandler;
        public LoginService(
            RepositoryContext db,
            ITokenService tokenService,
            IAuthCredentialsService authCredentialsService,
            IJwtHandler jwtHandler
        )
        {
            _db = db;
            _tokenService = tokenService;
            _authCredentialsService = authCredentialsService;
            _jwtHandler = jwtHandler;
        }
        
        public async Task<JwtWebTokenModel> SignIn(string email, string password)
        {
            //todo encrypt & decrypt 
            var authCredentials = await _db.AuthCredentials.FindAsync(email);
            if (authCredentials == null || authCredentials.Password != password)
                throw new BadRequestException();

            return await _tokenService.Create(email);
        }

        public async Task SignOut(string email, string refreshToken)
        {
            if (await _authCredentialsService.GetCredentials(email) == null)
            {
                throw new Exception($"The user with email = {email} does not exist");
            }

            await _tokenService.RevokeAccess(_jwtHandler.GetTokenFromHeader());
            await _tokenService.RevokeRefresh(email);
        }
    }
}