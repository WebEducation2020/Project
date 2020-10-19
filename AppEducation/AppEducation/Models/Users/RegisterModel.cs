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
        public string Birthday { get; set; }
        [Required]
        public string Job { get; set; }
        [Required]
        public bool Sex { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

    }
}