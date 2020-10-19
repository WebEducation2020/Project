
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppEducation.Models.Users{
    public class AppIdentityDbContext : IdentityDbContext<AppUser> {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Classes>()
                .HasKey(t => t.ClassID);
            base.OnModelCreating(builder);
        }
        public DbSet<Classes> Classes { get; set; }
    }
}