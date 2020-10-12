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
namespace AppEducation.Controllers {
    public class AccountController : Controller 
    {
       
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
        #endregion
    }
}