using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Project.Models
{
    public class BookingModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Select a Doctor")]
        public String DoctorName { get; set; }

        [Required]
        [Display(Name = "Select a Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime BookingDate { get; set; }

        [Required]
        [Display(Name = "Select a Time")]
        public TimeSpan BookingTime { get; set; }
    }
}