using System;
using ApiNogotochki.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiNogotochki.Repository
{
    public class RepositoryContext : DbContext
    {
        public DbSet<DbService> ServiceElements { get; set; }
        
        public DbSet<DbUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("API_NOGOTOCHKI_POSTGRES_CONNECTION_STRING"));
        }
    }
}