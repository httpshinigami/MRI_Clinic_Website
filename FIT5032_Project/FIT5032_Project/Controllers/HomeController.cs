using FIT5032_Project.CustomAttributes;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FIT5032_Project.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace FIT5032_Project.Controllers
{
    [Authorize]
    [SecurityHeader]
    [RequireHttps]
    public class HomeController : Controller
    {

        private DatabaseContext db = new DatabaseContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}