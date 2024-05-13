using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using tourfinal.Models;
using tourfinal.Email;
using tourfinal.Classes;


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
                    if (!string.IsNullOrEmpty(user.code))
                    {
                       
                        return RedirectToAction("VerifyOTP", new { userId = user.UserID, returnUrl = ReturnUrl });
                    }
                    else
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
        [AllowAnonymous]
        public ActionResult Signup(User u)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(user => user.UserName == u.UserName))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                }

                if (db.Users.Any(user => user.Email == u.Email))
                {
                    ModelState.AddModelError("Email", "Email address already exists.");
                }

                if (ModelState.IsValid)
                {
                    Random random = new Random();
                    int otp = random.Next(100000, 999999); 

                    // Save OTP to user's record in the database
                    u.code = otp.ToString(); // save the otp to table User, column code
                    db.Users.Add(u);
                    db.SaveChanges();

                   
                    string mailBody = $"Your OTP for signup is: {otp}";
                    MailManager mailManager = new MailManager();
                    string errorResponse = "";
                    bool emailSent = mailManager.SendEmail(u.Email, "Signup OTP", mailBody, ref errorResponse);

                    if (emailSent)
                    {
                        // Redirect to OTP verification page
                        
                        return RedirectToAction("VerifyOTP", new { userId = u.UserID });
                    }
                    else
                    {
                        ViewBag.Error = "Error sending email: " + errorResponse;
                        return View(u);
                    }
                }
            }

          
            return View(u);
        }


        [AllowAnonymous]
        public ActionResult VerifyOTP(int userId)
        {
           
            return View(new VerifyOTPViewModel { UserId = userId });
        }

        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifyOTP(int userId, string otp)
        {
            var user = db.Users.FirstOrDefault(u => u.UserID == userId && u.code == otp);

            if (user != null)
            {
              
                user.code = null;
                db.SaveChanges();

                TempData["SuccessMessage"] = "OTP verification successful. You can now log in.";
                return RedirectToAction("Login"); // Redirect to login page after OTP verification
            }
            else
            {
                ModelState.AddModelError("OTP", "Invalid OTP. Please try again.");
                TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
                return RedirectToAction("VerifyOTP", new { userId = userId });
            }
        }





        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["uname"] = null;
            return Redirect("Login");
        }
        [AllowAnonymous]
        public ActionResult Email()
        {
            ViewBag.Error = "";
            return View();
        }
        [AllowAnonymous]
        [HttpPost, ValidateInput(false)]
        public ActionResult Email(String mailTo, String mailBody, String mailSubject)
        {
            String strResult = String.Empty;
            MailManager mail = new MailManager();
            Random random = new Random();

            int otp = random.Next(100000, 999999); // Generate a 6-digit OTP
            var user = db.Users.FirstOrDefault(u => u.Email == mailTo);
            if (user != null)
            {
                user.code = otp.ToString();
                db.SaveChanges(); // Save OTP to the user's record in the database
            }

            // Append OTP message to the mail bodys
            mailBody += "<br/><br/>Your OTP is: " + otp;

            mail.SendEmail(mailTo, mailSubject, mailBody, ref strResult);
            ViewBag.Error = strResult;
            return View();
        }
       
        
    }
}
