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
using Microsoft.AspNetCore.Authentication;
namespace AppEducation.Controllers {
    public class AccountController : Controller 
    {
        private AppIdentityDbContext context;
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly ILogger<AccountController> logger;

        public AccountController(AppIdentityDbContext context, SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> passwordHasher,  IUserValidator<AppUser> userValidator, IPasswordValidator<AppUser> passwordValidator ,UserManager<AppUser> userManager, ILogger<AccountController> logger)
        {
            this.context = context;
            this.passwordValidator = passwordValidator;
            this.passwordHasher = passwordHasher;
            this.userValidator = userValidator;
            this.userManager = userManager;
            this.logger = logger;
            this.signInManager = signInManager;
        }
        [Authorize(Roles = "Student,Teacher")]
        public IActionResult Index() => RedirectToAction("Profile");
        
        #region Register method
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
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
                UserProfile profile = new UserProfile
                {
                    Birthday = model.Birthday,
                    Job = model.Job,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    Sex = model.Sex,
                    Email = model.Email
                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // save info user
                    AppUser usernew = await userManager.FindByNameAsync(user.UserName);
                    profile.UserId = usernew.Id;
                    context.UserProfiles.Add(profile);
                    if(profile.Job == "Teacher"){
                        var roleResult  = await userManager.AddToRoleAsync(usernew,"Teacher");
                    }else{
                        var roleResult = await userManager.AddToRoleAsync(usernew, "Student");
                    }
                    context.SaveChanges();
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
        [AllowAnonymous]
        public IActionResult Login(string returnUrl){
            ViewBag.returnUrl = returnUrl ;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel , string returnUrl) {
            if(string.IsNullOrEmpty(loginModel.UserName)) {
                ModelState.AddModelError("", "Please enter your email or username");
            }

            if(ModelState.IsValid){
                AppUser user = await userManager.FindByNameAsync(loginModel.UserName);
                if(user != null){
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user,loginModel.Password,false,false);
                    if(result.Succeeded) {
                        if(user.UserName == "admin")
                            return RedirectToAction("Index","Admin");
                        else
                            return Redirect(returnUrl ?? "/Account/Profile");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email), "Invalid user or password");
            }
            return View(loginModel);
          
        }
        #endregion 
        [Authorize(Roles = "Student,Teacher")]
        public async Task<IActionResult> Profile(){
        
            AppUser currentUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            UserProfile profile =  context.UserProfiles.First( p => p.UserId == currentUser.Id ) ;
            return View(profile);
        }
        [Authorize(Roles = "Student,Teacher")]
        public IActionResult ChangeProfile(UserProfile profile){
            if(ModelState.IsValid){
                UserProfile  old = context.UserProfiles.First( p => p.Email == profile.Email);
                if(old != null)
                {
                    old.FullName = profile.FullName;
                    old.Birthday = profile.Birthday;
                    old.PhoneNumber = profile.PhoneNumber;
                    old.Job = profile.Job;
                    old.Sex = profile.Sex;
                    context.SaveChanges();
                }
             
            }
            return RedirectToAction("Profile");
        }
        
    }
}