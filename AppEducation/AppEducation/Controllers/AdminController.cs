using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AppEducation.Models.Users;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AppEducation.Models;
using System.Collections.Generic;
namespace AppEducation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private TotalInformation totalInfo;
        private List<Room> rooms;
        public AdminController(UserManager<AppUser> usrMgr, List<Room> rms)
        {
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

        public JsonResult DetailAsJson(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return Json(user.Profile);
            }
            else
            {
                return Json("");
            }
        }

        // Delete User 
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
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