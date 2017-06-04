using Covalence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Covalence.Authentication
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

            modelBuilder.Entity<StudyUserTag>()
                .HasKey(x => new { x.UserId, x.Name });

            modelBuilder.Entity<ExpertUserTag>()
                .HasKey(x => new { x.UserId, x.Name });

            modelBuilder.Entity<StudyUserTag>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.StudyTags)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<ExpertUserTag>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.ExpertTags)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<StudyUserTag>()
                .HasOne(ut => ut.Tag)
                .WithMany(t => t.StudyUsers)
                .HasForeignKey(ut => ut.Name);

            modelBuilder.Entity<ExpertUserTag>()
                .HasOne(ut => ut.Tag)
                .WithMany(t => t.ExpertUsers)
                .HasForeignKey(ut => ut.Name);
        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<StudyUserTag> StudyUserTags { get; set; }
        public DbSet<ExpertUserTag> ExpertUserTags { get; set; }
        public DbSet<ExpertUserTag> ExpertUsers { get; set; }
        public DbSet<StudyUserTag> StudyUsers { get; set; }
        //public DbSet<Connection> Connections { get; set; }
    }
}