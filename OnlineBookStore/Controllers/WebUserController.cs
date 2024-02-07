using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using OnlineBookStore.Models.ViewModels;

namespace OnlineBookStore.Controllers
{
    public class WebUserController : Controller
    {
        OnlineBookStoreContext odb;
        UserManager<WebUser> userMan;
        public WebUserController(OnlineBookStoreContext o, UserManager<WebUser> um)
        {
            odb = o;
            userMan = um;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new WebUser());
        }
        [HttpPost]
        public async Task<IActionResult> Register(WebUser w)
        {
            if (!ModelState.IsValid)
            {
                return View(w);
            }
            var result = await userMan.CreateAsync(w, w.PasswordHash);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View(w);
        }
        [HttpGet]
        public IActionResult ChangePw()
        {
            //get the details of the user currently logged in
            //Send the userID to form - return VIew(u);
            WebUser w = odb.WebUsers.FirstOrDefault();
            if(w == null)
            {
                return NotFound("No User");
            }
            ChangePWVM cpw = new ChangePWVM();
            cpw.WebUserId = Convert.ToInt32(w.Id);
            return View(cpw);

           
        }
        [HttpPost]
        public IActionResult ChangePW(ChangePWVM cpw)
        {
            if (!ModelState.IsValid)
            {
                return View(cpw);
            }
            //check if new password and confirm password match
            if (cpw.ConfirmPassword != cpw.NewPassword)
            {
                ModelState.AddModelError("NewPassword", "New Password and confirm password do not match");
                return View(cpw);
            }
            //check if the user exists
            //get user info
            WebUser w = odb.WebUsers.Where(x => x.Id == cpw.WebUserId.ToString()).FirstOrDefault();
            if(w == null)
            {
                return NotFound("User does not exist");
            }

            //Check if old password matches
            if(cpw.OldPassword != w.PasswordHash)
            {
                ModelState.AddModelError("OldPassword", "Incorrect Old Password");
                return View(cpw);
            }

            //update the password
            w.PasswordHash = cpw.NewPassword;
            //save the changes
            odb.WebUsers.Update(w);
            odb.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
    }
}
