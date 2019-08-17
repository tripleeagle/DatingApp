using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Api.Auth.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Services.Interfaces
{
    public interface ILoginService
    {
        Task<JwtWebTokenModel> SignIn(string email, string password);
        Task SignOut(string email);
    }
}