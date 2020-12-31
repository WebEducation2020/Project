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
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
namespace AppEducation.Controllers
{

    [Authorize]
    public class JoinClassController : Controller
    {
        private readonly AppIdentityDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ConnectionHub> _hubContext;
        private UserManager<AppUser> userManager;
        public JoinClassController(ILogger<HomeController> logger, AppIdentityDbContext context, IHubContext<ConnectionHub> hubContext, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _hubContext = hubContext;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AvailableClasses(int pageNumber = 1)
        {
            IQueryable<Classes> classes = _context.Classes.Where(c => c.isActive == true); 
            JoinClassInfor joinClassInfor = new JoinClassInfor();
            joinClassInfor.AvailableClasses = classes;
            joinClassInfor.AvailableClasses.ToList().ForEach(c => {
                c.User = _context.Users.SingleOrDefault(u => u.Id == c.UserId);
                c.HOC = _context.HOClasses.SingleOrDefault(u => u.hocID == c.ClassID);
            });
            joinClassInfor.PageIndex = pageNumber;
            PaginatedList<Classes> page = new PaginatedList<Classes>(classes.ToList(),classes.Count(),joinClassInfor.PageIndex, 6);
            joinClassInfor.AvailableClasses = await page.CreateAsync(classes, joinClassInfor.PageIndex, 6);
            joinClassInfor.TotalPages = page.TotalPages;
            return View(joinClassInfor);
        }
        public async Task<IActionResult> Search(string InfoClass , int pageNumber = 1)
        {
            IQueryable<Classes> classes = _context.Classes.Where(c => c.ClassName.Contains(InfoClass) && c.isActive == true).Select(c => c);
            if(classes.Count() != 0)
            {
                JoinClassInfor joinClassInfor = new JoinClassInfor();
                joinClassInfor.AvailableClasses = classes;
                joinClassInfor.AvailableClasses.ToList().ForEach(c => {
                    c.User = _context.Users.SingleOrDefault(u => u.Id == c.UserId);
                    c.HOC = _context.HOClasses.SingleOrDefault(u => u.hocID == c.ClassID);
                });
                joinClassInfor.PageIndex = pageNumber;
                PaginatedList<Classes> page = new PaginatedList<Classes>(classes.ToList(), classes.Count(), joinClassInfor.PageIndex, 6);
                joinClassInfor.AvailableClasses = await page.CreateAsync(classes, joinClassInfor.PageIndex, 6);
                joinClassInfor.TotalPages = page.TotalPages;
                return View(joinClassInfor);
            }
            IQueryable<Classes> classesTopic = _context.Classes.Where(c => c.Topic.Contains(InfoClass) && c.isActive == true).Select(c => c);
            if (classesTopic.Count() != 0)
            {
                JoinClassInfor joinClassInfor = new JoinClassInfor();
                joinClassInfor.AvailableClasses = classesTopic;
                joinClassInfor.AvailableClasses.ToList().ForEach(c => {
                    c.User = _context.Users.SingleOrDefault(u => u.Id == c.UserId);
                    c.HOC = _context.HOClasses.SingleOrDefault(u => u.hocID == c.ClassID);
                });
                joinClassInfor.PageIndex = pageNumber;
                PaginatedList<Classes> page = new PaginatedList<Classes>(classesTopic.ToList(), classesTopic.Count(), joinClassInfor.PageIndex, 6);
                joinClassInfor.AvailableClasses = await page.CreateAsync(classesTopic, joinClassInfor.PageIndex, 6);
                joinClassInfor.TotalPages = page.TotalPages;
                return View(joinClassInfor);
            }
            IQueryable<Classes> classesTeacher = _context.Classes.Where(c => c.User.UserName.Contains(InfoClass) && c.isActive == true).Select(c => c);
            if (classesTeacher.Count() != 0)
            {
                JoinClassInfor joinClassInfor = new JoinClassInfor();
                joinClassInfor.AvailableClasses = classesTeacher;
                joinClassInfor.AvailableClasses.ToList().ForEach(c => {
                    c.User = _context.Users.SingleOrDefault(u => u.Id == c.UserId);
                    c.HOC = _context.HOClasses.SingleOrDefault(u => u.hocID == c.ClassID);
                });
                joinClassInfor.PageIndex = pageNumber;
                PaginatedList<Classes> page = new PaginatedList<Classes>(classesTeacher.ToList(), classesTeacher.Count(), joinClassInfor.PageIndex, 6);
                joinClassInfor.AvailableClasses = await page.CreateAsync(classesTeacher, joinClassInfor.PageIndex, 6);
                joinClassInfor.TotalPages = page.TotalPages;
                return View(joinClassInfor);
            }
            IQueryable<Classes> classesAll = _context.Classes.Where(c => c.isActive == true);
            if(InfoClass == null)
            {
                JoinClassInfor joinClassInfor_ = new JoinClassInfor();
                joinClassInfor_.AvailableClasses = classesAll;
                joinClassInfor_.AvailableClasses.ToList().ForEach(c => {
                    c.User = _context.Users.SingleOrDefault(u => u.Id == c.UserId);
                    c.HOC = _context.HOClasses.SingleOrDefault(u => u.hocID == c.ClassID);
                });
                joinClassInfor_.PageIndex = pageNumber;
                PaginatedList<Classes> page_ = new PaginatedList<Classes>(classesAll.ToList(), classesAll.Count(), joinClassInfor_.PageIndex, 6);
                joinClassInfor_.AvailableClasses = await page_.CreateAsync(classesAll, joinClassInfor_.PageIndex, 6);
                joinClassInfor_.TotalPages = page_.TotalPages;
                return View(joinClassInfor_);
            }
            JoinClassInfor joinClassInfor_1 = new JoinClassInfor();
            joinClassInfor_1.AvailableClasses = classesAll;
            return View(joinClassInfor_1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create(JoinClassInfor joinClassInfor)
        {
            if (ModelState.IsValid)
            {
                AppUser currentUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                joinClassInfor.NewClass.User = currentUser;
                HistoryOfClass hoc = new HistoryOfClass
                {
                    hocID = joinClassInfor.NewClass.ClassID,
                    startTime = DateTime.Now,
                };
                joinClassInfor.NewClass.isActive = true;
                joinClassInfor.NewClass.HOC = hoc;
                _context.Classes.Add(joinClassInfor.NewClass);
                await _context.SaveChangesAsync();
                WriteCookies("ClassName", joinClassInfor.NewClass.ClassName, true);
                WriteCookies("ClassID", joinClassInfor.NewClass.ClassID, true);
                WriteCookies("Topic", joinClassInfor.NewClass.Topic, true);
                return RedirectToAction("Present", "JoinClass", joinClassInfor.NewClass);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student,Teacher")]
        public IActionResult Join(JoinClassInfor joinClassInfor)
        {
            if (ModelState.IsValid)
            {
                Classes cls = _context.Classes.Find(joinClassInfor.NewClass.ClassID);
                if (cls == null)
                    return RedirectToAction("Create", "JoinClass", joinClassInfor);
                else
                {
                    WriteCookies("ClassName", cls.ClassName, true);
                    WriteCookies("ClassID", cls.ClassID, true);
                    WriteCookies("Topic", cls.Topic, true);
                    cls.OnlineStudent += 1;
                    _context.SaveChanges();
                    return RedirectToAction("Present", "JoinClass", cls);
                }
            }
            return RedirectToAction("Create", "JoinClass", joinClassInfor);
        }
        [Authorize]
        public IActionResult Present(Classes cls)
        {
            Classes oldClass = ReadCookies();
            if (cls.ClassName == null)
            {
                if (oldClass != null)
                {
                    return View(oldClass);
                }
                else
                {
                    return RedirectToAction("Create", "JoinClass");
                }
            }
            return View(cls);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }
        #region Cookies 
        public void WriteCookies(string setting, string settingValue, bool isPersistent)
        {
            if (isPersistent)
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddMinutes(60);
                Response.Cookies.Append(setting, settingValue, options);
            }
            else
            {
                Response.Cookies.Append(setting, settingValue);
            }
            ViewBag.Message = "Cookie Written Successfully!";
        }
        public Classes ReadCookies()
        {
            var ClassInfo = new Classes();
            ClassInfo.ClassName = Request.Cookies["ClassName"];
            ClassInfo.ClassID = Request.Cookies["ClassID"];
            ClassInfo.Topic = Request.Cookies["Topic"];
            return ClassInfo;
            // hihihi
        }
        #endregion 

    }
}