using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tourfinal.Models;

namespace tourfinal.Controllers
{
    public class Booking1Controller : Controller
    {
        private TsuperVansEntities db = new TsuperVansEntities();

        // GET: Booking1
        public ActionResult Index()
        {
            var bookings1 = db.Bookings1.Include(b => b.DestinationDetail);
            return View(bookings1.ToList());
        }

        // GET: Booking1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking1 booking1 = db.Bookings1.Find(id);
            if (booking1 == null)
            {
                return HttpNotFound();
            }
            return View(booking1);
        }

        // GET: Booking1/Create
        public ActionResult Create()
        {
            ViewBag.DestinationID = new SelectList(db.DestinationDetails, "DestinationID", "DestinationName");
            return View();
        }

        // POST: Booking1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Booking_Date,Booking_Number,Number_of_Travellers,Type_of_Booking,Package_Booked,Price,DestinationID,BookingID")] Booking1 booking1)
        {
            if (ModelState.IsValid)
            {
                db.Bookings1.Add(booking1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DestinationID = new SelectList(db.DestinationDetails, "DestinationID", "DestinationName", booking1.DestinationID);
            return View(booking1);
        }

        // GET: Booking1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking1 booking1 = db.Bookings1.Find(id);
            if (booking1 == null)
            {
                return HttpNotFound();
            }
            ViewBag.DestinationID = new SelectList(db.DestinationDetails, "DestinationID", "DestinationName", booking1.DestinationID);
            return View(booking1);
        }

        // POST: Booking1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Booking_Date,Booking_Number,Number_of_Travellers,Type_of_Booking,Package_Booked,Price,DestinationID,BookingID")] Booking1 booking1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DestinationID = new SelectList(db.DestinationDetails, "DestinationID", "DestinationName", booking1.DestinationID);
            return View(booking1);
        }

        // GET: Booking1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking1 booking1 = db.Bookings1.Find(id);
            if (booking1 == null)
            {
                return HttpNotFound();
            }
            return View(booking1);
        }

        // POST: Booking1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking1 booking1 = db.Bookings1.Find(id);
            db.Bookings1.Remove(booking1);
            db.SaveChanges();
            return RedirectToAction("Index");
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
