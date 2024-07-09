namespace FIT5032_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RatingToBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingModels", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.BookingModels", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingModels", "Comment");
            DropColumn("dbo.BookingModels", "Rating");
        }
    }
}
