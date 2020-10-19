
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace AppEducation.Models.Users
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Classes> Classes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Classes>()
                .HasKey(t => t.ClassID);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>().ToTable("UserProfiles");
            modelBuilder.Entity<UserProfile>().HasKey(t => t.UserProfileId);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Profile).WithMany(w => w.Users)
                .HasForeignKey(u => u.UserProfileId).IsRequired(false);
        }

    }
}