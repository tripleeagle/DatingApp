using System;
using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Models.Exceptions;
using Api.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Auth.Services
{
    public class LoginService: ILoginService
    {
        private readonly RepositoryContext _db;
        private readonly ITokenService _tokenService;

        public LoginService(RepositoryContext db,ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }
        
        public async Task<JwtWebTokenModel> SignIn(string email, string password)
        {
            //todo encrypt & decrypt 
            var authCredentials = await _db.AuthCredentials.FindAsync(email);
            if (authCredentials == null || authCredentials.Password != password)
                throw new BadRequestException();

            return await _tokenService.Create(email);
        }

        /*public IActionResult SignOut(string email)
        {
            
        }*/
    }
}