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

            List<string> doctorNames = GetDoctorNames();
            ViewBag.DoctorNames = doctorNames;

            string currentUserId = User.Identity.GetUserId();
            ViewBag.Author = currentUserId;
            return View();

        }

        public List<string> GetDoctorNames()
        {
            List<string> doctorNames = new List<string>();

            string connectionStringName = "DefaultConnection"; // Specify the desired connection string name
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT FirstName + ' ' + LastName AS DoctorName FROM AspNetUserRoles " +
                    "nur JOIN AspNetUsers nu ON nur.UserId = nu.Id WHERE nur.RoleId = '1e06f578-828b-4fd1-b6c6-0fb928513ca0';";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string doctorName = reader["DoctorName"].ToString();
                            doctorNames.Add(doctorName);
                        }
                    }
                }
            }

            return doctorNames;
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}