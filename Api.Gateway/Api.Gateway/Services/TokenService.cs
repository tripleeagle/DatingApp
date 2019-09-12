using System.Linq;
using System.Threading.Tasks;
using Api.Gateway.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;

namespace Api.Gateway.Services
{
    public class TokenService: ITokenService
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<bool> IsCurrentActiveToken()
            => await IsActive(GetTokenFromHeader());
        
        
        private async Task<bool> IsActive(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }
            return await _cache.GetStringAsync(token) == null;
        }
        
        private string GetTokenFromHeader()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }
    }
}