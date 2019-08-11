using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Data.Enums;
using Api.Auth.Extensions;
using Api.Auth.Models;
using Api.Auth.Models.Exceptions;
using Api.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Api.Auth.Services
{
    public class LoginService: ILoginService
    {
        private readonly RepositoryContext _db;
        private readonly AuthConfig _authConfig;
        private readonly IJwtService _jwtService;

        public LoginService(RepositoryContext db, IOptions<AuthConfig> authConfig, IJwtService jwtService)
        {
            _db = db;
            _authConfig = authConfig.Value;
            _jwtService = jwtService;
        }
        
        public async Task<IActionResult> Login(string email, string password)
        {
            //todo encrypt & decrypt 
            var authCredentials = await _db.AuthCredentials.FindAsync(email);
            if (authCredentials == null || authCredentials.Password != password)
                throw new BadRequestException();
            

            var expireTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_authConfig.JwtExpireMinutes));
            return new JsonResult(JsonConvert.SerializeObject(GenerateToken(email,expireTime))) {StatusCode = 200};
        }

        public IActionResult Logout(string email)
        {
            return new JsonResult(JsonConvert.SerializeObject(GenerateToken(email,DateTime.Now))) {StatusCode = 200};
        }

        public IActionResult Refresh(string email, JwtSecurityToken jwtSecurityToken)
        {
            if (jwtSecurityToken.Claims.FirstOrDefault(x =>
                    x.Type == ClaimsIdentity.DefaultNameClaimType && x.Value == email) == null)
                return null;
            var expireTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_authConfig.JwtExpireMinutes));
            return new JsonResult(JsonConvert.SerializeObject(GenerateToken(email,expireTime))) {StatusCode = 200};
        }
        private string GenerateToken(string email, DateTime expireTime)
        {
            var jwt = new JwtSecurityToken(
                _authConfig.Issuer,
                _authConfig.Audience,
                notBefore: DateTime.UtcNow,
                claims: GetClaims(email),
                expires: expireTime,
                signingCredentials: new SigningCredentials(
                    _jwtService.GetSymmetricSecurityKey(), 
                    SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);  
        }
        
        private static IEnumerable<Claim> GetClaims(string email)
        {
            return new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, GroupTypesEnum.User.Description())
            };
        }
    }
}