using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Auth.Data;
using Api.Auth.Data.Enums;
using Api.Auth.Extensions;
using Api.Auth.Models;
using Api.Auth.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Api.Auth.Services
{
    public class JwtHandler: IJwtHandler
    {
        private readonly AuthConfig _authConfig;
        
        public JwtHandler(IOptions<AuthConfig> authConfig)
        {
            _authConfig = authConfig.Value;
        }
        
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authConfig.Key));
        }

        public JwtWebTokenModel Generate(string email)
        {
            var expiresAccess = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_authConfig.JwtExpireMinutes));
            var expiresRefresh = DateTime.UtcNow.Add(TimeSpan.FromDays(_authConfig.RefreshTokenExpireMonth * 30));
            var jwtAccess = new JwtSecurityToken(
                _authConfig.Issuer,
                _authConfig.Audience,
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
                        _authConfig.Issuer,
                        _authConfig.Audience,
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