using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FIT5032_Project.Models
{
    public partial class MapModels : DbContext
    {
        public MapModels()
            : base("name=MapModels")
        {
        }

        public virtual DbSet<LocationModel> Locations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationModel>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LocationModel>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<LocationModel>()
                .Property(e => e.Latitude)
                .HasPrecision(10, 8);

            modelBuilder.Entity<LocationModel>()
                .Property(e => e.Longitude)
                .HasPrecision(11, 8);
        }
    }
}
