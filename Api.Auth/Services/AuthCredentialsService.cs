using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Data.Enums;
using Api.Auth.Services.Interfaces;

namespace Api.Auth.Services
{
    public class AuthCredentialsService: IAuthCredentialsService
    {
        private readonly RepositoryContext _db;
        public AuthCredentialsService(RepositoryContext db)
        {
            _db = db;
        }

        public async Task Create(AuthCredentials authCredentials)
        {
            authCredentials.RegistrationStateEnum = RegistrationStateEnum.Unconfirmed;
            await _db.AuthCredentials.AddAsync(authCredentials);
        }
    }
}