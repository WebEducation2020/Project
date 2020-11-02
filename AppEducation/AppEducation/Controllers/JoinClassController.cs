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
using AppEducation.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AppEducation.Controllers
{

    [Authorize]
    public class JoinClassController : Controller
    {
        private readonly AppIdentityDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ConnectionHub> _hubContext;

        public JoinClassController(ILogger<HomeController> logger, AppIdentityDbContext context, IHubContext<ConnectionHub> hubContext )
        {
            _logger = logger;
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
                
            return View();
        }
        public IActionResult Create()
        {
            IEnumerable<Classes> classes = _context.Classes;
            JoinClassInfor joinClassInfor = new JoinClassInfor();
            joinClassInfor.AvailableClasses = classes;
            return View(joinClassInfor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Teacher")]
        public async Task<IActionResult> Create(JoinClassInfor joinClassInfor)
        {
            if (ModelState.IsValid)
            {
                _context.Classes.Add(joinClassInfor.NewClass);
                await _context.SaveChangesAsync();
                return RedirectToAction("Present", "JoinClass", joinClassInfor.NewClass);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Student,Teacher")]
        public IActionResult Join(JoinClassInfor joinClassInfor)
        {
            if (ModelState.IsValid)
            {
                Classes cls = _context.Classes.Find(joinClassInfor.NewClass.ClassID);
                if (cls == null)
                    return RedirectToAction("Create","JoinClass", joinClassInfor);
                return RedirectToAction("Present", "JoinClass", cls);
            }
            return RedirectToAction("Create","JoinClass", joinClassInfor);
        }
        //[Authorize]
        [AllowAnonymous]
        public IActionResult Present(Classes cls)
        {
            return View(cls);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
