using Microsoft.EntityFrameworkCore;

namespace Api.Auth.Data
{
    public class RepositoryContext: DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlUseIdentityColumns();
        }
        
        public DbSet<AuthCredentials> AuthCredentials { get; set; }
        public DbSet<JwtRefreshToken> JwtRefreshTokens { get; set; }
    }
}