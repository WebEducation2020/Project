
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace AppEducation.Models.Users{
    public class AppIdentityDbContext : IdentityDbContext<AppUser> {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) {}
    
        public DbSet<UserProfile> UserProfiles {get; set;}

        public DbSet<Classes> Classes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<AppUser>( entity => 
            {
                entity.ToTable(name:"Users");
            });
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<Classes>()
                .HasKey(t => t.ClassID);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>().ToTable("UserProfiles");
            modelBuilder.Entity<UserProfile>().HasKey( t => t.UserProfileId);

            modelBuilder.Entity<AppUser>()
                .HasOne( u => u.Profile)
                .WithOne( p => p.User)
                .HasForeignKey<UserProfile>( p => p.UserId);
        }
    
    }
}