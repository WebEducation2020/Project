using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AppEducation.Models.Users;
using System.Threading.Tasks;
namespace AppEducation.Controllers {
    public class AdminController: Controller {
        private UserManager<AppUser> userManager;
        public AdminController(UserManager<AppUser> usrMgr){
            userManager = usrMgr;
        }
        public ViewResult Index() => View(userManager.Users);

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