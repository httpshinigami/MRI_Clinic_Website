using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Project.Models
{
    public class RatingModel
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}