using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppEducation.Models.Users{
    public class AppIdentityDbContext : IdentityDbContext<AppUser> {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) {}
    }
}