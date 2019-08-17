using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Api.Auth.Data;
using Api.Auth.Data.Enums;
using Api.Auth.Extensions;
using Api.Auth.Models;
using Api.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Api.Auth.Services
{
    public class JwtHandler: IJwtHandler
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public JwtHandler(IOptions<JwtOptions> authConfig, IHttpContextAccessor httpContextAccessor)
        {
            _jwtOptions = authConfig.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Key));
        }

        public JwtWebTokenModel Generate(string email)
        {
            var expiresAccess = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtOptions.JwtExpireMinutes));
            var expiresRefresh = DateTime.UtcNow.Add(TimeSpan.FromDays(_jwtOptions.RefreshTokenExpireMonth * 30));
            var jwtAccess = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                notBefore: DateTime.UtcNow,
                claims: GetClaims(email),
                expires: expiresAccess,
                signingCredentials: new SigningCredentials(
                    GetSymmetricSecurityKey(), 
                    SecurityAlgorithms.HmacSha256)
            );
            var jwtRefreshToken = new JwtRefreshToken{
                Email = email,
                JwtToken = JsonConvert.SerializeObject(
                    new JwtSecurityToken(
                        _jwtOptions.Issuer,
                        _jwtOptions.Audience,
                        notBefore: DateTime.UtcNow,
                        claims: GetClaims(email),
                        expires: expiresRefresh,
                        signingCredentials: new SigningCredentials(
                            GetSymmetricSecurityKey(), 
                            SecurityAlgorithms.HmacSha256)
                    )),
                ValidTo = expiresRefresh
            };
            
            return new JwtWebTokenModel
            {
                AccessToken = JsonConvert.SerializeObject(jwtAccess),
                JwtRefreshToken = jwtRefreshToken
            };
        }

        public string GetTokenFromHeader()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
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