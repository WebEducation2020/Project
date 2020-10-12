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
        private readonly ILogger<AccountController> logger;

        public AccountController( ILogger<AccountController> logger)
        {
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
            if(ModelState.IsValid){
                AppUser user = new AppUser {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                IdentityResult result = await userManager.CreateAsync(user,model.Password);
                if(result.Succeeded) {
                    return RedirectToAction("Index");
                }else{
                    foreach(IdentityError error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        #endregion
    }
}