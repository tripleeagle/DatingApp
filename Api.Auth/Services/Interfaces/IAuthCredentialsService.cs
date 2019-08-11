using System.Threading.Tasks;
using Api.Auth.Data;

namespace Api.Auth.Services.Interfaces
{
    public interface IAuthCredentialsService
    {
        Task Create(string email, string password);
        Task ChangePassword(string newPassword, string email, string password);
        Task Delete(string email, string password);
    }
}