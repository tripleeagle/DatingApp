using System;
using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Models;
using Api.Auth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Api.Auth.Services
{
    public class TokenService: ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly RepositoryContext _db;
        private readonly IJwtHandler _jwtHandler;
        private readonly IDistributedCache _cache;

        public TokenService(
            IOptions<JwtOptions> jwtOptions, 
            RepositoryContext db,
            IJwtHandler jwtHandler, 
            IDistributedCache cache
        )
        {
            _jwtOptions = jwtOptions.Value;
            _db = db;
            _jwtHandler = jwtHandler;
            _cache = cache;
        }
        
        public async Task<JwtWebTokenModel> Refresh(string refreshToken, string email)
        {
            var refreshTokenFromDb = await GetRefreshToken(refreshToken);
            if (refreshTokenFromDb == null)
            {
                throw new Exception("Refresh token was not found");
            }

            if (refreshTokenFromDb.ValidTo < DateTime.Now)
            {
                throw new Exception("Refresh token was revoked");
            }
            
            return await Create(email);
        }

        public async Task<JwtWebTokenModel> Create(string email)
        {
            var jwtWebTokenModel = _jwtHandler.Generate(email);
            await _db.JwtRefreshTokens.AddAsync(jwtWebTokenModel.JwtRefreshToken);
            await _db.SaveChangesAsync();
            return jwtWebTokenModel;
        }
        
        public async Task RevokeAccess(string accessToken)
        {
            await _cache.SetStringAsync(accessToken, "", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(_jwtOptions.JwtExpireMinutes)
            });
        }

        public async Task RevokeRefresh(string email)
        {
            var refreshTokenFromDb = await GetRefreshTokenByEmail(email);
            if (refreshTokenFromDb == null)
            {
                throw new Exception("Refresh token was not found");
            }

            if (refreshTokenFromDb.ValidTo < DateTime.Now)
            {
                throw new Exception("Refresh token was already revoked");
            }
            refreshTokenFromDb.ValidTo = DateTime.MinValue;
            await _db.SaveChangesAsync();
        }
        
        public async Task<bool> IsActive(string token)
        {
            return await _cache.GetStringAsync(token) == null;
        }

        private Task<JwtRefreshToken> GetRefreshTokenByEmail(string email)
        {
            return _db.JwtRefreshTokens.FirstOrDefaultAsync(x => x.Email == email);
        }
        
        private Task<JwtRefreshToken> GetRefreshToken(string refreshToken)
        {
            return _db.JwtRefreshTokens.FirstOrDefaultAsync(x =>
                x.JwtToken == refreshToken);
        }
    }
}