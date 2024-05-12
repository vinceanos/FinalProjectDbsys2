using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tourfinal.Models;
using tourfinal.Email;


namespace tourfinal.Controllers
{
    public class HomeController : Controller
    {
       
        
            private readonly TsuperVansEntities db; // Use a private readonly field

            public HomeController()
            {
                db = new TsuperVansEntities(); // Initialize in the constructor
            }

        // GET: Home
        [Authorize(Roles = "Admin,Customer")]

        public ActionResult Index()
        {
            var destinations = db.DestinationDetails.ToList();
            return View(destinations);
        }
        [Authorize(Roles = "Admin,Customer")]


        public ActionResult DestinationDetail(int? id)
            {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var destination = db.DestinationDetails.FirstOrDefault(d => d.DestinationID == id);
            var booking = db.Bookings.FirstOrDefault(b => b.DestinationID == id);

            if (destination == null || booking == null)
            {
                return RedirectToAction("Index");
            }

            var viewModel = new DestinationBookingViewModel
            {
                Destination = destination,
                Booking = booking as Booking1
            };

            return View(viewModel);
        }

        public ActionResult BookNow(int destinationID)
        {
            var booking = new Booking { DestinationID = destinationID };
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookNow(Booking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Bookings.Add(booking);

                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("BookingSuccess");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while processing your booking. Please try again later.");
                }
            }

            return View(booking);
        }


        // GET: Home/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult UserManagement()
        {
            var user = db.Users.ToList();
            return View(user);

        }
        [Authorize(Roles = "Admin")]
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Service()
        {
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Password,Email,RoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

       
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Password,Email,RoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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

            // Append OTP message to the mail body
            mailBody += "<br/><br/>Your OTP is: " + otp;

            mail.SendEmail(mailTo, mailSubject, mailBody, ref strResult);
            ViewBag.Error = strResult;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
