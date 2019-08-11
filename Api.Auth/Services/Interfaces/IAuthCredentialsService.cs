using System.Threading.Tasks;
using Api.Auth.Data;

namespace Api.Auth.Services.Interfaces
{
    public interface IAuthCredentialsService
    {
        Task Create(AuthCredentials authCredentials);
    }
}