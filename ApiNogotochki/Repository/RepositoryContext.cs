using ApiNogotochki.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiNogotochki.Repository
{
    public class RepositoryContext : DbContext
    {
        private readonly string connectionString;

        public RepositoryContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
        
        public DbSet<DbService> Services { get; set; }
        
        public DbSet<DbUser> Users { get; set; }

        public DbSet<DbPhoto> Photos { get; set; }
        
        public DbSet<DbSearchIndexRecord> SearchIndices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbPhoto>().HasKey(x => new
            {
                x.Id,
                x.Size,
            });
            modelBuilder.Entity<DbSearchIndexRecord>().HasKey(x => new
            {
                x.TargetId,
                x.TargetType,
                x.ValueType,
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}