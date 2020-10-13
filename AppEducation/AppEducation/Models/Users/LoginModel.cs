using System.ComponentModel.DataAnnotations;
using System;

namespace AppEducation.Models.Users {
    public class LoginModel {
        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}