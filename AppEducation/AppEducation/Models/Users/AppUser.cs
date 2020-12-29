using Microsoft.AspNetCore.Identity;

namespace AppEducation.Models.Users{
    public class AppUser : IdentityUser {
        public UserProfile Profile {get; set;}
        public Notification Notifications { get; set; }
    }
}