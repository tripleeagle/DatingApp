using System.Threading.Tasks;

namespace Api.Gateway.Services.Interfaces
{
    public interface ITokenService
    {
        Task<bool> IsCurrentActiveToken();
    }
}