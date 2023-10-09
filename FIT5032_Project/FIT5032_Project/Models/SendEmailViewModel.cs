using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FIT5032_Project.Models
{
    public class SendEmailViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Please enter a subject.")]
        [Display(Name = "Subject:")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter a message")]
        [Display(Name = "Contents:")]
        public string Contents { get; set; }

        [Display(Name = "Attachment:")]
        public HttpPostedFileBase Attachment { get; set; }
    }
}