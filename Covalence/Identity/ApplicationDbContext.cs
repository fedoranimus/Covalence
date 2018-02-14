using Covalence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Covalence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tag>()
                .HasKey(x => new { x.Name });

            modelBuilder.Entity<UserTag>()
                .HasKey(x => new { x.UserId, x.Name });

            modelBuilder.Entity<Location>()
                .HasKey(x => new { x.Latitude, x.Longitude });

            modelBuilder.Entity<UserTag>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.Tags)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<UserTag>()
                .HasOne(ut => ut.Tag)
                .WithMany(t => t.Users)
                .HasForeignKey(ut => ut.Name);

            modelBuilder.Entity<Connection>()
                .HasKey(c => new { c.RequestingUserId, c.RequestedUserId });

            modelBuilder.Entity<Connection>()
                .HasOne(c => c.RequestingUser)
                .WithMany()
                .HasForeignKey(c => c.RequestingUserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Connection>()
                .HasOne(c => c.RequestedUser)
                .WithMany()
                .HasForeignKey(c => c.RequestedUserId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Users)
                .WithOne(u => u.Location);
        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}