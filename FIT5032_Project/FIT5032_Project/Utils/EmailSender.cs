using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;


namespace FIT5032_Project.Utils
{
    public class EmailSender
    {
        private const String API_KEY = "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";

        public async Task SendAsync(String toEmail, String subject, String contents, byte[] fileBytes = null, string fileName = null)
        {
            try
            {
                var client = new SendGridClient(API_KEY);
                var from = new EmailAddress("mche0012@student.monash.edu", "Sakura MRI");
                var to = new EmailAddress(toEmail, "");
                var plainTextContent = contents;
                var htmlContent = "<p>" + contents + "</p>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                Debug.WriteLine("fileBytes:", fileBytes);

                // Add the attachment if fileBytes is provided
                if (fileBytes != null && fileBytes.Length > 0)
                {
                    var base64Content = Convert.ToBase64String(fileBytes);
                    msg.AddAttachment(fileName, base64Content);
                }

                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Debug.WriteLine("SendGrid API returned a non-OK status code: " + response.StatusCode);
                }
                else
                {
                    Debug.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while sending the email: " + ex.Message);
                throw; // Rethrow the exception to handle it in your controller.
            }
        }
    }
}