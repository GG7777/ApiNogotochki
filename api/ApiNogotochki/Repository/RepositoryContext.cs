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

			Database.EnsureCreated();
		}

		public DbSet<DbService> Services { get; set; }
		
		public DbSet<DbServiceMeta> ServicesMetas { get; set; }

		public DbSet<DbUser> Users { get; set; }

		public DbSet<DbPhoto> Photos { get; set; }
		
		public DbSet<DbMessage> Messages { get; set; }

		public DbSet<DbDialog> Dialogs { get; set; }

		public DbSet<DbSearchIndexRecord> SearchIndices { get; set; }

		public DbSet<DbGeolocationIndexRecord> GeolocationIndices { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DbSearchIndexRecord>().HasKey(x => new
			{
				x.TargetId,
				x.TargetType,
				x.ValueType
			});
			modelBuilder.Entity<DbGeolocationIndexRecord>().HasKey(x => new
			{
				x.TargetId,
				x.TargetType
			});
			modelBuilder.Entity<DbDialog>().HasAlternateKey(x => new
			{
				x.ServiceId,
				x.UserId,
			});
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(connectionString);
		}
	}
}