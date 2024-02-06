using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using OnlineBookStore.Models.ViewModels;

namespace OnlineBookStore.Controllers
{
    public class WebUserController : Controller
    {
        OnlineBookStoreContext odb;

        public WebUserController(OnlineBookStoreContext o)
        {
            odb = o;
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
            cpw.WebUserId = w.WebUserId;
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
            WebUser w = odb.WebUsers.Where(x => x.WebUserId == cpw.WebUserId).FirstOrDefault();
            if(w == null)
            {
                return NotFound("User does not exist");
            }

            //Check if old password matches
            if(cpw.OldPassword != w.Pw)
            {
                ModelState.AddModelError("OldPassword", "Incorrect Old Password");
                return View(cpw);
            }

            //update the password
            w.Pw = cpw.NewPassword;
            //save the changes
            odb.WebUsers.Update(w);
            odb.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
    }
}
