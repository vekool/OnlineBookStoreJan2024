using Microsoft.AspNetCore.Authorization;
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
        SignInManager<WebUser> signInMan;
        public WebUserController(OnlineBookStoreContext o, UserManager<WebUser> um, SignInManager<WebUser> s)
        {
            odb = o;
            userMan = um;
            signInMan = s;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new WebUser());
        }
        public IActionResult ForgotPass()
        {
            return View(new ForgotPassVM());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPass(ForgotPassVM fp)
        {
            if (!ModelState.IsValid)
            {
                return View(fp);
            }

            var user = await userMan.FindByEmailAsync(fp.Email);

            if(user == null)
            {
                return NotFound();
            }
            var token = await userMan.GeneratePasswordResetTokenAsync(user);
            var passWordResetLink = Url.Action("ResetPassword", "Webuser", new { token, email = user.Email });
            ViewBag.PLink = passWordResetLink;
            //send email to user
            //show mail sent page
            return View("ForgotPasswordLinkView");
        }
        //Anonymus -> 
        //Normal Users who have logged in --> Amazon (Customer)
        [HttpGet]
       
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginVM());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM lvm)
        {
            if (!ModelState.IsValid)
            {
                return View(lvm);
            }
            var result = await signInMan.PasswordSignInAsync(lvm.UserName, lvm.Password, lvm.RememberMe, false);
            if (result.Succeeded)
            {
                TempData["cartitemms"] = 0;
                //some code here
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Credentials");
                return View(lvm);
            }

        }
        //only allow logged in users to logout
        
        public async Task<IActionResult> Logout()
        {
            await signInMan.SignOutAsync();
            return RedirectToAction("Index", "Home");

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
