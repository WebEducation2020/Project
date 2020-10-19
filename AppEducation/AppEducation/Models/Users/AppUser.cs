using Microsoft.AspNetCore.Identity;

namespace AppEducation.Models.Users{
    public class AppUser : IdentityUser {
        public long UserProfileId {get;set;}
        public UserProfile Profile {get; set;}

        
    }
}