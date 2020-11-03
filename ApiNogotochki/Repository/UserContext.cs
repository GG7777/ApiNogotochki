using ApiNogotochki.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiNogotochki.Repository
{
    public class UserContext : DbContext
    {
        public DbSet<UserMeta> UserMetas { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserSocial> UserSocials { get; set; }
        public DbSet<UserGeolocation> UserGeolocations { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=api_nogotochki;Username=postgres;Password=password");
        }
    }
}