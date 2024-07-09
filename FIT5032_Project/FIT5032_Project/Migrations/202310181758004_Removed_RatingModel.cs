namespace FIT5032_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removed_RatingModel : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RatingModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RatingModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.String(),
                        Author = c.String(),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
