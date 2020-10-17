using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppEducation.Models;
using Microsoft.AspNetCore.Authorization;
using AppEducation.Models.Users;

namespace AppEducation.Controllers
{


    public class JoinClassController : Controller
    {
        private readonly AppIdentityDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public JoinClassController(ILogger<HomeController> logger, AppIdentityDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Classes _class)
        {
            if (ModelState.IsValid)
            {
                _context.Classes.Add(_class);
                _context.SaveChanges();
                return RedirectToAction("Present", "Home", _class);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Join(Classes _class)
        {
            if (ModelState.IsValid)
            {
                Classes cls = _context.Classes.Find(_class.ClassID);
                if (cls == null)
                    return View();
                return RedirectToAction("Index", "Present", cls);
            }
            return View();
        }
        [AllowAnonymous]
        public IActionResult Present()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
