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

namespace AppEducation.Controllers
{
    public class AccountController : Controller
    {
        private AppIdentityDbContext context;
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly ILogger<AccountController> logger;

        public AccountController(AppIdentityDbContext context, SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> passwordHasher, IUserValidator<AppUser> userValidator, IPasswordValidator<AppUser> passwordValidator, UserManager<AppUser> userManager, ILogger<AccountController> logger)
        {
            this.context = context;
            this.passwordValidator = passwordValidator;
            this.passwordHasher = passwordHasher;
            this.userValidator = userValidator;
            this.userManager = userManager;
            this.logger = logger;
            this.signInManager = signInManager;
        }
        [Authorize(Roles = "Student, Teacher")]
        public IActionResult Index() => RedirectToAction("Profile");

        #region Register method
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.Birthday))
            {
                ModelState.AddModelError(nameof(model.Birthday), "Please enter your birthday");
            }
            if (ModelState.GetValidationState("Date") == ModelValidationState.Valid && DateTime.Now > Convert.ToDateTime(model.Birthday))
            {
                ModelState.AddModelError(nameof(model.Birthday), "Please enter a date in the past");
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Please enter your email");
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
                    FullName = model.UserName,
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
                    var roleResult = await userManager.AddToRoleAsync(usernew, "Student");
                    context.SaveChanges();
                    return RedirectToAction("Index", "Home");
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
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserName))
            {
                ModelState.AddModelError("", "Please enter your email or username");
            }
            if (string.IsNullOrEmpty(loginModel.Password))
            {
                ModelState.AddModelError("", "Please enter your password");
            }
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(loginModel.UserName);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (user.UserName == "admin")
                        {
                            return RedirectToAction(loginModel.RequestPath ?? "Index", "Admin");
                        }
                        else
                            return Redirect(loginModel.RequestPath ?? "/JoinClass/AvailableClasses");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email), "Invalid user or password");
            }
            return View(loginModel);

        }
        #endregion 

        #region Logout
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion 

        public async Task<IActionResult> Profile()
        {
            AppUser currentUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (currentUser != null)
            {
                try
                {
                    var profile = context.UserProfiles.First(p => p.UserId == currentUser.Id);
                    if (profile != null)
                        return View(profile);
                }
                catch
                {
                    UserProfile newProfile = new UserProfile();
                    newProfile.UserId = currentUser.Id;
                    return View(newProfile);
                };
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student,Teacher")]
        public async Task<IActionResult> ChangeProfile(UserProfile profile)
        {
            if (ModelState.IsValid)
            {
                AppUser currentUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                
                UserProfile old = context.UserProfiles.First(p => p.UserId == currentUser.Id);
                if (old != null)
                {
                    if( old.FullName != profile.FullName)
                    {
                        old.FullName = profile.FullName;
                    }
                    if( old.Birthday != profile.Birthday)
                    {
                        old.Birthday = profile.Birthday;
                    }
                    if( old.PhoneNumber != profile.PhoneNumber)
                    {
                        currentUser.PhoneNumber = profile.PhoneNumber;
                    }
                    if( old.Job != profile.Job)
                    {
                        old.Job = profile.Job;
                    }
                    if(old.Sex != profile.Sex)
                        old.Sex = profile.Sex;
                    if (old.Password != profile.Password)
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(currentUser);

                        var res = await userManager.ResetPasswordAsync(currentUser, token, profile.Password);
                        if (res.Succeeded)
                        {
                            old.Password = profile.Password;
                        }
                    }
                    context.SaveChanges();
                    var result = await userManager.UpdateAsync(currentUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Change not success!");
                    }
                }

            }
            return RedirectToAction("Profile");
        }
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }
        [AllowAnonymous]
        public IActionResult AccessDenied(string error)
        {
            return View(error);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/JoinClass/AvailableClasses");
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor:true);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
            {
                AppUser user = await userManager.FindByEmailAsync(info.Principal.FindFirst(ClaimTypes.Email).Value);
                if(user.UserName == "admin")
                {
                    return LocalRedirect("~/Admin/Index");
                }
                return LocalRedirect(returnUrl);
            }
            else
            {
                AppUser user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identResult = await userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false);
                        var roleResult = await userManager.AddToRoleAsync(user, "Student");
                        return LocalRedirect(returnUrl);
                    }
                }
                return RedirectToAction("AccessDenied");
            }
        }
    }
}