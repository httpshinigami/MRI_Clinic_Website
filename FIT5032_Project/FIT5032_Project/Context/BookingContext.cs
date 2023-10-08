using FIT5032_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FIT5032_Project.Context
{
    public class BookingContext : DbContext
    {
        public DbSet <BookingModel> Booking { get; set; }
    }
}