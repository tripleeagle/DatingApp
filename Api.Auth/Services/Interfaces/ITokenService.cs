using System.Threading.Tasks;
using Api.Auth.Data;

namespace Api.Auth.Services.Interfaces
{
    public interface ITokenService
    {
        Task<JwtWebTokenModel> Refresh(string refreshToken, string email);
        Task<JwtWebTokenModel> Create(string email);
    }
}