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
                        // If the user has an OTP pending verification, redirect to the OTP verification page
                        return RedirectToAction("VerifyOTP", new { userId = user.UserID, returnUrl = ReturnUrl });
                    }
                    else
                    {
                        // If the user does not have an OTP pending verification, log them in
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
                    int otp = random.Next(100000, 999999); // Generate a 6-digit OTP

                    // Save OTP to user's record in the database
                    u.code = otp.ToString(); // Assign OTP to the 'code' property
                    db.Users.Add(u);
                    db.SaveChanges();

                    // Send OTP via email
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

            // If ModelState is not valid or any other condition fails, return the view with the current model
            return View(u);
        }


        [AllowAnonymous]
        public ActionResult VerifyOTP(int userId)
        {
           
            return View(new VerifyOTPViewModel { UserId = userId });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifyOTP(VerifyOTPViewModel model)
        {
            var user = db.Users.FirstOrDefault(u => u.UserID == model.UserId);

            if (user != null && TempData["OTP"] != null && user.code != null && user.code.Equals(TempData["OTP"].ToString()))
            {
                // If OTP is correct, remove the OTP from the user's record and proceed with signup
                user.code = null;
                db.SaveChanges();

                // Save the user to the database
                db.Users.Add(user);
                db.SaveChanges();


                return RedirectToAction("Login"); // Redirect to login page after OTP verification
            }
           
                return View(model);
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
