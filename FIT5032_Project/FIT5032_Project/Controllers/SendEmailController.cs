using FIT5032_Project.Models;
using FIT5032_Project.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FIT5032_Project.Controllers
{
    public class SendEmailController : Controller
    {
        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Send_Email(SendEmailViewModel model, HttpPostedFileBase Attachment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string toEmail = model.ToEmail;
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

                    // Send email using SendGrid without an emailMessage object
                    EmailSender es = new EmailSender();
                    await es.SendAsync(toEmail, subject, contents, fileBytes, fileName);

                    ViewBag.Result = "Email has been sent.";

                    ModelState.Clear();

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
