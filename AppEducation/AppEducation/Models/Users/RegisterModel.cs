using System.ComponentModel.DataAnnotations;
using System;
namespace AppEducation.Models.Users{
    public class RegisterModel {
        [Required]
        public string UserName {get; set;}
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string BirthDay { get; set; }
        [Required]
        public string Job { get; set; }
        [Required]
        public int Sex { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        
    }
}