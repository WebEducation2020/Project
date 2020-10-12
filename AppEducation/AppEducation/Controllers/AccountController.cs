using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AppEducation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using AppEducation.Models.Users;
using Microsoft.AspNetCore.Identity;
namespace AppEducation.Controllers {
    public class AccountController : Controller 
    {
       
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly ILogger<AccountController> logger;

        public AccountController(  IPasswordHasher<AppUser> passwordHasher,  IUserValidator<AppUser> userValidator, IPasswordValidator<AppUser> passwordValidator ,UserManager<AppUser> userManager, ILogger<AccountController> logger)
        {
            this.passwordValidator = passwordValidator;
            this.passwordHasher = passwordHasher;
            this.userValidator = userValidator;
            this.userManager = userManager;
            this.logger = logger;
        }

        public IActionResult Index() => View();
        
        #region Login method
        public IActionResult Login() => View();
        #endregion
         #region Register method
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model) {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        #endregion
    }
}