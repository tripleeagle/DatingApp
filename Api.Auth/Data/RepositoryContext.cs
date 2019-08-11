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
            modelBuilder.Entity<AuthCredentials>(entity =>
            {
                entity.ToTable("Auth_Credentials");
                entity.HasKey(x => x.Email);
            });
        }
        
        public DbSet<AuthCredentials> AuthCredentials { get; set; }
    }
}