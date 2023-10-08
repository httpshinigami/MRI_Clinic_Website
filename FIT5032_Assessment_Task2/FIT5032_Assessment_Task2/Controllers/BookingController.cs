using FIT5032_Assessment_Task2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FIT5032_Assessment_Task2.Controllers
{
    public class BookingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        // GET: Booking/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        public ActionResult Create(Booking model)
        {
            try
            {
                String DoctorName = model.DoctorName;
                DateTime BookingTime = model.BookingTime;
                ViewBag.BookingDetail = DoctorName + " " + BookingTime;

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Booking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Booking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
