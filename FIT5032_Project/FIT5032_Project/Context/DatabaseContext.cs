using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace FIT5032_Project.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        {
            // Constructor: Configure the database connection (replace "YourConnectionString" with your connection string name)
        }
        public DbSet <BookingModel> Bookings { get; set; }
        public DbSet <LocationModel> Locations { get; set; }

        public System.Data.Entity.DbSet<FIT5032_Project.Models.ImageModel> ImageModels { get; set; }
    }
}
