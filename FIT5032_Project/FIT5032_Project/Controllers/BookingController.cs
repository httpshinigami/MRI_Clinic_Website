using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using FIT5032_Project.CustomAttributes;
using FIT5032_Project.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Rotativa;
using System.Xml.Linq;

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
        public string GetPatientName()
        {
            string connectionStringName = "DefaultConnection";
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            string currentUserId = User.Identity.GetUserId();
            string name = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT FirstName FROM AspNetUsers WHERE Id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", currentUserId);

                    // Use ExecuteScalar to retrieve a single value
                    name = command.ExecuteScalar() as string;
                }
            }

            return name;
        }


        //Generate PDF and download it
        public ActionResult GeneratePDF()
        {
            var model = getBookings();
            return new Rotativa.ViewAsPdf("Index", model)
            {
                FileName = "YourBookings.pdf"
            };
        }
        public IEnumerable<FIT5032_Project.Models.BookingModel> getBookings()
        {
            string currentUserId = User.Identity.GetUserId();
            return db.Bookings.Where(m => m.Author == currentUserId).ToList();
        }

        public ActionResult ExportToExcel()
        {
            // Get the data you want to export
            var data = getBookings();

            // Set the license context for the ExcelPackage
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string patientName = GetPatientName();
            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Add a worksheet to the package
                var worksheet = package.Workbook.Worksheets.Add(patientName);

                worksheet.Cells[1, 1].Value = "Doctor Name";
                worksheet.Cells[1, 2].Value = "Date";
                worksheet.Cells[1, 3].Value = "Time";
                // Fill the worksheet with data
                for (int i = 2; i <= data.Count(); i++)
                {
                    worksheet.Cells[i, 1].Value = data.ElementAt(i - 1).DoctorName;
                    worksheet.Cells[i, 2].Value = data.ElementAt(i - 1).BookingDate.ToString("MM/dd/yyyy");
                    worksheet.Cells[i, 3].Value = data.ElementAt(i - 1).BookingTime.ToString();
                    // Add more columns as needed
                }

                // Set the style of the header row (optional)
                worksheet.Cells["A1:C1"].Style.Font.Bold = true;
                worksheet.Cells["A1:C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:C1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                // Auto-fit columns (optional)
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Save the Excel package to a MemoryStream
                using (var memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);

                    // Return the Excel file as a downloadable response
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "YourBookings.xlsx");
                }
            }
        }




        // Get: Chart
        [Authorize(Roles = "Admin,Staff,Doctor")]
        public ActionResult Chart()
        {

            DateTime today = DateTime.Today;
            DayOfWeek currentDayOfWeek = today.DayOfWeek;
            int daysUntilStartOfWeek = (int)DayOfWeek.Monday - (int)currentDayOfWeek;
            int daysUntilEndOfWeek = 6 - (int)currentDayOfWeek;

            DateTime startDate = today.AddDays(daysUntilStartOfWeek);
            DateTime endDate = today.AddDays(daysUntilEndOfWeek).AddHours(23).AddMinutes(59).AddSeconds(59);

            List<BookingModel> weekBookings = db.Bookings.Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate).ToList();

            var groupedBookings = weekBookings.GroupBy(b => b.BookingDate.Date)
                .OrderBy(group => group.Key) // Sort by date in ascending order
                .Select(group => new
                {
                    Date = group.Key.ToShortDateString(),
                    Count = group.Count()
                })
                .ToList();

            List<BookingInfo> weekBookings2 = groupedBookings
                .Select(group => new BookingInfo
                {
                    Date = group.Date,
                    Count = group.Count
                })
                .ToList();

            // Chart.js requires a JSON object (to make it, we need to SerializeObject)
            ViewBag.weekBookingsJSON = JsonConvert.SerializeObject(weekBookings2);

            // For the current month
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            List<BookingModel> monthBookings = db.Bookings
                .Where(b => b.BookingDate >= startOfMonth && b.BookingDate <= endOfMonth)
                .ToList();

            var groupedMonthBookings = monthBookings
                .GroupBy(b => b.BookingDate.Date)
                .OrderBy(group => group.Key) // Sort by date in ascending order
                .Select(group => new
                {
                    Date = group.Key.ToShortDateString(),
                    Count = group.Count()
                })
                .ToList();

            List<BookingInfo> monthBookings2 = groupedMonthBookings
                .Select(group => new BookingInfo
                {
                    Date = group.Date,
                    Count = group.Count
                })
                .ToList();

            ViewBag.monthBookingsJSON = JsonConvert.SerializeObject(monthBookings2);

            // For the current year
            DateTime startOfYear = new DateTime(today.Year, 1, 1);
            DateTime endOfYear = new DateTime(today.Year, 12, 31).AddHours(23).AddMinutes(59).AddSeconds(59);

            List<BookingModel> yearBookings = db.Bookings
                .Where(b => b.BookingDate >= startOfYear && b.BookingDate <= endOfYear)
                .ToList();

            var groupedYearBookings = yearBookings
                .GroupBy(b => b.BookingDate.Date)
                .OrderBy(group => group.Key) // Sort by date in ascending order
                .Select(group => new
                {
                    Date = group.Key.ToShortDateString(),
                    Count = group.Count()
                })
                .ToList();

            List<BookingInfo> yearBookings2 = groupedYearBookings
                .Select(group => new BookingInfo
                {
                    Date = group.Date,
                    Count = group.Count
                })
                .ToList();

            ViewBag.yearBookingsJSON = JsonConvert.SerializeObject(yearBookings2);

            return View(weekBookings);
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


        // GET: Booking/Book
        public ActionResult Book(String date, string DoctorId)
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

        //HTTP GET Booking/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(BookingModel Model)
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
                                Rating = Convert.ToDouble(reader["AggRating"])
                            });

                        }
                    }
                }
            }

            return doctors;
        }

    }
}
