using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_Project.Context;
using FIT5032_Project.CustomAttributes;
using FIT5032_Project.Models;

namespace FIT5032_Project.Controllers
{
    [Authorize]
    [SecurityHeader]
    public class BookingController : Controller
    {
        private BookingContext db = new BookingContext();

        // GET: Booking
        public ActionResult Index()
        {
            return View(db.Booking.ToList());
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingModel bookingModel = db.Booking.Find(id);
            if (bookingModel == null)
            {
                return HttpNotFound();
            }
            return View(bookingModel);
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DoctorName,BookingDate,BookingTime")] BookingModel bookingModel)
        {
            if (ModelState.IsValid)
            {
                db.Booking.Add(bookingModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookingModel);
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingModel bookingModel = db.Booking.Find(id);
            if (bookingModel == null)
            {
                return HttpNotFound();
            }
            return View(bookingModel);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DoctorName,BookingDate,BookingTime")] BookingModel bookingModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookingModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookingModel);
        }

        // GET: Booking/Delete/5
        [Authorize(Roles = "Admin, Doctor, Staff")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingModel bookingModel = db.Booking.Find(id);
            if (bookingModel == null)
            {
                return HttpNotFound();
            }
            return View(bookingModel);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Doctor, Staff")]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingModel bookingModel = db.Booking.Find(id);
            db.Booking.Remove(bookingModel);
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
