using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using tourfinal.Models;

namespace tourfinal.Controllers
{
    public class AccountController : Controller
    {
        TsuperVansEntities db = new TsuperVansEntities()
        {
        }; 
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(User u, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == u.UserName && x.Password == u.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(u.UserName, false);
                    Session["uname"] = u.UserName;
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                }
            }
            return RedirectToAction("Login");
        }    
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User u)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(u);
                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Login");
                }
            }   
            return View();
        }
        public ActionResult RegisterDriver()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterDriver(DriverUser m)
        {
            if (ModelState.IsValid)
            {
                var driverUser = new DriverUser
                {
                    // Populate other properties of the driver user from the registration form
                    DriverLicenseNumber = m.DriverLicenseNumber,
                    VehicleModel = m.VehicleModel,
                    DestinationID = m.DestinationID  // SelectedDestinationID comes from the form
                };

                db.DriverUsers.Add(driverUser);
                db.SaveChanges();

                // Redirect to a success page or another action
                return RedirectToAction("RegistrationSuccess");
            }

            // If model state is not valid, return to the registration form with validation errors
            return View(m);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["uname"] = null;
            return Redirect("Login");
        }
    }
}
