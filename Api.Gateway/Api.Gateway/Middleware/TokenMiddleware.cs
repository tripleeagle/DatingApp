using System;
using System.Threading.Tasks;
using Api.Gateway.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Api.Gateway.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ITokenService tokenService)
        {
            if (!await tokenService.IsCurrentActiveToken())
            {
                throw new Exception("The token was revoked");
            }
            await _next(httpContext);
        }
    }
}