namespace FIT5032_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class LocationModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayFormat(DataFormatString = "{0:###.########}")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayFormat(DataFormatString = "{0:###.########}")]
        public decimal Longitude { get; set; }
    }
}
