using System.Threading.Tasks;
using Api.Auth.Data;
using Api.Auth.Data.Enums;
using Api.Auth.Models.Exceptions;
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

        public async Task Create(string email, string password)
        {
            if (await _db.AuthCredentials.FindAsync(email) != null) 
                throw new BadRequestException();
            var authCredentials = new AuthCredentials(email, password);
            await _db.AuthCredentials.AddAsync(authCredentials);
            await _db.SaveChangesAsync();
        }
        
        public async Task ChangePassword(string newPassword, string email, string password)
        {
            var authCredentialsDb = await _db.AuthCredentials.FindAsync(email);
            if ( authCredentialsDb != null && authCredentialsDb.Password != password)
                throw new BadRequestException();

            if (authCredentialsDb != null)
            {
                authCredentialsDb.Password = newPassword;
                await _db.SaveChangesAsync();
                return;
            }
            
            await Create(email, newPassword);
        }
        
        public async Task Delete(string email, string password)
        {
            var authCredentialsDb = await _db.AuthCredentials.FindAsync(email);
            if (authCredentialsDb == null || password != authCredentialsDb.Password)
                throw new BadRequestException();
            _db.AuthCredentials.Remove(authCredentialsDb);
            await _db.SaveChangesAsync();
        }

        public Task<AuthCredentials> GetCredentials(string email)
        {
            return _db.AuthCredentials.FindAsync(email);
        }
    }
}