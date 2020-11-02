using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AppEducation.Models.Users;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AppEducation.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Linq;
namespace AppEducation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private TotalInformation totalInfo;
        private AppIdentityDbContext context;
        private List<Room> rooms;
        public AdminController(UserManager<AppUser> usrMgr, List<Room> rms, AppIdentityDbContext ctx)
        {
            context = ctx;
            userManager = usrMgr;
            rooms = rms;
        }
        public ViewResult Index()
        {
            totalInfo = new TotalInformation();
            totalInfo.Users = userManager.Users;
            totalInfo.Rooms = rooms;
            return View(totalInfo);
        }
        public ViewResult UserManager() => View(userManager.Users);

        public async Task<JsonResult> detailasjson(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            
            if (user != null)
            {
                UserProfile profile =  context.UserProfiles.First( p => p.UserId == user.Id) ; 
                string data =  JsonConvert.SerializeObject(profile);
                return Json(data);
            }
            else
            {
                return Json(null);
            }
        }

        // Delete User 
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            IQueryable<UserProfile> profiles =  context.UserProfiles.Where(p => p.UserId == user.Id);
            foreach( UserProfile profile  in profiles ){
                context.UserProfiles.Remove(profile);
            }
            await context.SaveChangesAsync();
            if (user != null)
            {   
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                   
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}