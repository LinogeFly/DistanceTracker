using DistanceTracker.Data.Model;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace DistanceTracker.Data.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<History> History { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasIndex(x => x.UserName);
            modelBuilder.Entity<User>().Property(x => x.UserName).IsRequired(true);
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).IsRequired(true);
            modelBuilder.Entity<User>().Property(x => x.PasswordSalt).IsRequired(true);

            modelBuilder.Entity<History>().ToTable("History");
            modelBuilder.Entity<History>().HasKey(x => x.Id);
            modelBuilder.Entity<History>().Property(x => x.UserId).IsRequired(true);
            modelBuilder.Entity<History>().Property(x => x.Date).IsRequired(true);

            modelBuilder.Entity<Route>().ToTable("Routes");
            modelBuilder.Entity<Route>().HasKey(x => x.Id);
            modelBuilder.Entity<Route>().Property(x => x.UserId).IsRequired(true);
            modelBuilder.Entity<Route>().Property(x => x.StartLocation).IsRequired(true);
            modelBuilder.Entity<Route>().Property(x => x.EndLocation).IsRequired(true);
            modelBuilder.Entity<Route>().Property(x => x.Waypoints).IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
