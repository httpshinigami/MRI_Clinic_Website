namespace FIT5032_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Author : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingModels", "Author", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingModels", "Author");
        }
    }
}
