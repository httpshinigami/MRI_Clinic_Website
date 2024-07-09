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

        [Display(Name = "Select a Doctor")]
        public string DoctorId { get; set; }

        public string DoctorName { get; set; }
        public string Author { get; set; }

        [Required]
        [Display(Name = "Select a Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime BookingDate { get; set; }

        [Required]
        [Display(Name = "Select a Time")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan BookingTime { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}