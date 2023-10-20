using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FIT5032_Project.CustomAttributes;
using FIT5032_Project.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_Project.Controllers
{
    [Authorize]
    [SecurityHeader]
    public class BookingController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Booking

        public ActionResult Index()
        {
            //return View(db.Articles.ToList());
            string currentUserId = User.Identity.GetUserId();
            return View(db.Bookings.Where(m => m.Author == currentUserId).ToList());
        }

        // GET: Booking/Feedback/5
        public ActionResult Feedback(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingModel bookingModel = db.Bookings.Find(id);
            if (bookingModel == null)
            {
                return HttpNotFound();
            }
            return View(bookingModel);
        }
        // POST: Booking/Feedback/6
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(int? id, int? Rating, string comment)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookingModel bookingModel = db.Bookings.Find(id);

            if (bookingModel == null)
            {
                return HttpNotFound();
            }

            // Here, you can work with the form data as needed.
            // For example, you can update the booking model with the rating and comment.
            bookingModel.Rating = Rating ?? 0;
            bookingModel.Comment = comment;

            // Save the changes to the database.
            db.Entry(bookingModel).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the index page or another appropriate action.
        }


        public List<DoctorInfoModel> GetDoctorNames()
        {
            List<DoctorInfoModel> doctors = new List<DoctorInfoModel>();

            string connectionStringName = "DefaultConnection"; // Specify the desired connection string name
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT nur.UserId as DoctorId, FirstName + ' ' + LastName AS DoctorName FROM AspNetUserRoles " +
                    "nur JOIN AspNetUsers nu ON nur.UserId = nu.Id WHERE nur.RoleId = '1e06f578-828b-4fd1-b6c6-0fb928513ca0';";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctors.Add(new DoctorInfoModel
                            {
                                DoctorId = reader["DoctorId"].ToString(),
                                Name = reader["DoctorName"].ToString()
                            });

                        }
                    }
                }
            }

            return doctors;
        }

        // GET: Booking/Create
        public ActionResult Create()
        {

            List<DoctorInfoModel> doctorNames = GetDoctorNames();
            ViewBag.DoctorList = new SelectList(doctorNames, "DoctorId", "Name");

            string currentUserId = User.Identity.GetUserId();
            ViewBag.Author = currentUserId;
            return View();

        }



        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookingModel bookingModel)
        {
            bookingModel.Author = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Bookings.Add(bookingModel);
                // add notify.js
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
            BookingModel bookingModel = db.Bookings.Find(id);
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
            BookingModel bookingModel = db.Bookings.Find(id);
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
            BookingModel bookingModel = db.Bookings.Find(id);
            db.Bookings.Remove(bookingModel);
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

        // GET: Booking/Calendar
        public ActionResult Calendar(string DoctorId)
        {
            // Retrieve all bookings from a DoctorId & put it in a list called DoctorBookings
            List<BookingModel> DoctorBookings = db.Bookings.Where(m => m.DoctorId == DoctorId).ToList();

            // 
            string currentUserId = User.Identity.GetUserId();
            ViewBag.Author = currentUserId;
            ViewBag.Doctor = DoctorId;
            return View(DoctorBookings);

        }

        // POST: Booking/Calendar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Calendar(BookingModel bookingModel)
        {
            bookingModel.Author = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Bookings.Add(bookingModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookingModel);
        }


        // GET: Booking/Gagan
        public ActionResult Gagan(String date, string DoctorId)
        {
            if (null == date)
            {
                return RedirectToAction("Index");
            }

            DateTime convertedDate = DateTime.Parse(date);
            ViewBag.date = convertedDate;
            ViewBag.DoctorId = DoctorId;
            string connectionStringName = "DefaultConnection";
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            string sqlQuery = "select FirstName + ' ' + Lastname as name, id from AspNetUsers nu where id = @DoctorId;";

            string userName = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.Add(new SqlParameter("@DoctorId", DoctorId));
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        userName = Convert.ToString(result);
                    }
                }
            }

            if (userName != null)
            {
                ViewBag.userName = userName;
            }
            else
            {
                // Handle the case when the user is not found
            }
            // Assuming db is  DbContext instance for Entity Framework
            List<BookingModel> bookedTimes = db.Bookings
                .Where(m => m.DoctorId == DoctorId && m.BookingDate == convertedDate)
                //.Select(m => m.BookingTime)
                .ToList();
            return View(bookedTimes);
        }

        //HTTP GET Booking/Gagan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Gagan(BookingModel Model)
        {
            List<BookingModel> bookedTimes = db.Bookings
                            .Where(m => m.DoctorId == Model.DoctorId && m.BookingDate == Model.BookingDate)
                            .ToList();
            bool isTimeSlotAvailable = !bookedTimes.Any(booking => booking.BookingTime == Model.BookingTime);

            if (ModelState.IsValid)
            {
                // Model state is valid, proceed with booking
                if ((Model.BookingDate > DateTime.Now.Date)             // If Booking is not today or a past date
                    && (Model.BookingTime > TimeSpan.FromHours(9))      // If Booking is after 9am
                    && (Model.BookingTime < TimeSpan.FromHours(18))     // If Booking is before 6pm
                    && isTimeSlotAvailable)                             // If Booking is available 
                {
                    var booking = new BookingModel
                    {
                        DoctorId = Model.DoctorId,
                        DoctorName = Model.DoctorName,
                        BookingDate = Model.BookingDate,
                        BookingTime = Model.BookingTime,
                        Author = User.Identity.GetUserId()
                    };              

                db.Bookings.Add(booking);
                db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(bookedTimes);
            }
        }


        public List<DoctorInfoModel> GetDoctorAppointments()
        {
            List<DoctorInfoModel> doctors = new List<DoctorInfoModel>();

            string connectionStringName = "DefaultConnection"; // Specify the desired connection string name
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT nur.UserId as DoctorId, FirstName + ' ' + LastName AS DoctorName FROM AspNetUserRoles " +
                    "nur JOIN AspNetUsers nu ON nur.UserId = nu.Id WHERE nur.RoleId = '1e06f578-828b-4fd1-b6c6-0fb928513ca0';";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctors.Add(new DoctorInfoModel
                            {
                                DoctorId = reader["DoctorId"].ToString(),
                                Name = reader["DoctorName"].ToString()
                            });

                        }
                    }
                }
            }

            return doctors;
        }

        [AllowAnonymous]
        public ActionResult Doctors()
        {
            ViewBag.Message = "Your application description page.";
            //return View(db.Articles.ToList());
            //return View(db.ImageModels.Where(m => m.Author == currentUserId).ToList());

            List<DoctorInfoModel> doctorNames = GetDoctorInfo();
            ViewBag.DoctorInfo = doctorNames;

            string currentUserId = User.Identity.GetUserId();
            ViewBag.Author = currentUserId;
            return View();

        }

        public List<DoctorInfoModel> GetDoctorInfo()
        {
            List<DoctorInfoModel> doctors = new List<DoctorInfoModel>();

            string connectionStringName = "DefaultConnection";
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT nur.UserId as DoctorId, FirstName + ' ' + LastName AS DoctorName, ROUND(AVG(CAST(Rating AS FLOAT)), 2) as AggRating FROM AspNetUserRoles nur JOIN AspNetUsers nu ON nur.UserId = nu.Id JOIN BookingModels b ON nu.Id = b.DoctorId WHERE nur.RoleId = '1e06f578-828b-4fd1-b6c6-0fb928513ca0' AND Rating > 0 GROUP BY nur.UserId, FirstName + ' ' + LastName; ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctors.Add(new DoctorInfoModel
                            {
                                DoctorId = reader["DoctorId"].ToString(),
                                Name = reader["DoctorName"].ToString(),
                                Rating = reader["AggRating"].ToString()
                            });

                        }
                    }
                }
            }

            return doctors;
        }

    }
}
