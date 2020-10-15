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
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace AppEducation.Controllers {
    public class AccountController : Controller 
    {
       
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly ILogger<AccountController> logger;

        public AccountController( SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> passwordHasher,  IUserValidator<AppUser> userValidator, IPasswordValidator<AppUser> passwordValidator ,UserManager<AppUser> userManager, ILogger<AccountController> logger)
        {
            this.passwordValidator = passwordValidator;
            this.passwordHasher = passwordHasher;
            this.userValidator = userValidator;
            this.userManager = userManager;
            this.logger = logger;
            this.signInManager = signInManager;
        }

        public IActionResult Index() => View();
        
        #region Register method
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model) {
            if(string.IsNullOrEmpty(model.Birthday)){
                ModelState.AddModelError(nameof(model.Birthday), "Please enter your birthday");
            }
            if(ModelState.GetValidationState("Date") == ModelValidationState.Valid && DateTime.Now > Convert.ToDateTime(model.Birthday)) {
                ModelState.AddModelError(nameof(model.Birthday), "Please enter a date in the past");
            }
            if(string.IsNullOrEmpty(model.Email)){
                ModelState.AddModelError(nameof(model.Email),"Please enter your email");

            }
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

        #region Login
        public IActionResult Login(string returnUrl){
            ViewBag.returnUrl = returnUrl ;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel , string returnUrl) {
            if(ModelState.IsValid){
                AppUser user = await userManager.FindByEmailAsync(loginModel.Email);
                if(user != null){
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user,loginModel.Password,false,false);
                    if(result.Succeeded) {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email), "Invalid user or password");
            }
            return View(loginModel);
          
        }
        #endregion 
    }
}