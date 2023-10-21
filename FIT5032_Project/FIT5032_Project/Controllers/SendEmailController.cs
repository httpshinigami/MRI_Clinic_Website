using FIT5032_Project.CustomAttributes;
using FIT5032_Project.Models;
using FIT5032_Project.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.BuilderProperties;
using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Policy;
using System.Web.Optimization;

namespace FIT5032_Project.Controllers
{
    [Authorize]
    [SecurityHeader]
    [Authorize(Roles = "Admin,Staff,Doctor")]
    public class SendEmailController : Controller
    {
        public ActionResult Send_Email()
        {
            List<UserInfoModel> users = GetUsers();
            ViewBag.UserList = new SelectList(users, "Email", "Name");
            return View(new SendEmailViewModel());
        }

        public List<UserInfoModel> GetUsers()
        {
            List<UserInfoModel> users = new List<UserInfoModel>();

            string connectionStringName = "DefaultConnection"; // Specify the desired connection string name
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id as UserId, FirstName + ' ' + LastName AS Name,Email FROM AspNetUsers ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserInfoModel
                            {
                                UserId = reader["UserId"].ToString(),
                                Email = reader["Email"].ToString(),
                                Name = reader["Name"].ToString()
                            });

                        }
                    }
                }
            }

            return users;
        }

        // POST: Send Email
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Send_Email(SendEmailViewModel model, HttpPostedFileBase Attachment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<string> toEmail = model.ToEmail;
                    string subject = model.Subject;
                    string contents = model.Contents;

                    byte[] fileBytes = null;
                    string fileName = null;

                    if (Attachment != null && Attachment.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(Attachment.FileName);

                        // Save the attachment to a temporary folder
                        var path = Server.MapPath("~/Attachments/") + fileName;
                        Attachment.SaveAs(path);


                        // Convert attachment to byte array for SendGrid
                        using (var memoryStream = new MemoryStream())
                        {
                            await Attachment.InputStream.CopyToAsync(memoryStream);
                            fileBytes = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        ViewBag.UploadResult = "File not uploaded.";
                    }
                    
                    foreach (string EmailAddress in toEmail)
                    {
                        System.Diagnostics.Debug.WriteLine("email", EmailAddress);
                        if (!string.IsNullOrEmpty(EmailAddress)) 
                        {
                            // Send email using SendGrid without an emailMessage object
                            EmailSender es = new EmailSender();
                            await es.SendAsync(EmailAddress, subject, contents, fileBytes, fileName);
                        }
                    }

                    ViewBag.Result = "Email has been sent.";

                    ModelState.Clear();

                    List<UserInfoModel> users = GetUsers();
                    ViewBag.UserList = new SelectList(users, "Email", "Name");

                    return View(new SendEmailViewModel());
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    ViewBag.Error = "An error occurred: " + ex.Message;
                    return View();
                }
            }

            return View();

        }

    }

}
