namespace FIT5032_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Location_Class : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Location");
        }
    }
}
