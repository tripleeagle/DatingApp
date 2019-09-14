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

namespace Api.Auth.Services
{
    public class JwtHandler: IJwtHandler
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ICryptoHandler _cryptoHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtHandler(
            IOptions<JwtSettings> authConfig, 
            ICryptoHandler cryptoHandler,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _jwtSettings = authConfig.Value;
            _cryptoHandler = cryptoHandler;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));
        }

        public JwtWebTokenModel Generate(string email)
        {
            var expiresAccess = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtSettings.JwtExpireMinutes));
            var expiresRefresh = DateTime.UtcNow.Add(TimeSpan.FromDays(_jwtSettings.RefreshTokenExpireMonth * 30));
            var jwtAccessToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                notBefore: DateTime.UtcNow,
                claims: GetClaims(email),
                expires: expiresAccess,
                signingCredentials: new SigningCredentials(
                    GetSymmetricSecurityKey(), 
                    SecurityAlgorithms.HmacSha256)
            );
            
            var encodedJwtAccess = new JwtSecurityTokenHandler().WriteToken(jwtAccessToken);
            
            return new JwtWebTokenModel
            {
                AccessToken = encodedJwtAccess,
                JwtRefreshToken = new JwtRefreshToken{
                    Email = email,
                    Key = _cryptoHandler.EncryptString(email),
                    ValidTo = expiresRefresh
                }
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