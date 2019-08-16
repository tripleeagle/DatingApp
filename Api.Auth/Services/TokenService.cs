using System;
using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Models.Exceptions;
using Api.Auth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Auth.Services
{
    public class TokenService: ITokenService
    {
        private readonly RepositoryContext _db;
        private readonly IJwtHandler _jwtHandler;
        
        public TokenService(RepositoryContext db, IJwtHandler jwtHandler)
        {
            _db = db;
            _jwtHandler = jwtHandler;
        }
        
        public async Task<JwtWebTokenModel> Refresh(string refreshToken, string email)
        {
            if (await GetRefreshToken(refreshToken, email) == null)
            {
                throw new BadRequestException();
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
        
        private Task<JwtRefreshToken> GetRefreshToken(string refreshToken, string email)
        {
            return _db.JwtRefreshTokens.FirstOrDefaultAsync(x =>
                x.JwtToken == refreshToken && 
                x.Email == email && 
                x.ValidTo > DateTime.Now);
        }
    }
}