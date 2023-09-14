using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Assessment_Task2.Models
{
    public class Booking
    {
        [Required]
        [Display(Name = "Select a doctor")]
        public String DoctorName { get; set; }
        [Display(Name = "Select a date & time")]
        public DateTime BookingTime { get; set; }
    }
}