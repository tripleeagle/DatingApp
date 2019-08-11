using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Services.Interfaces
{
    public interface ILoginService
    {
        Task<IActionResult> Login(string email, string password);
        IActionResult Logout(string email);
        IActionResult Refresh(string email, JwtSecurityToken jwtSecurityToken);
    }
}