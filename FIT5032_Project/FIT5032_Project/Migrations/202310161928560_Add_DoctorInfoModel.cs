namespace FIT5032_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DoctorInfoModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ImageModels", "Path", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.ImageModels", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ImageModels", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ImageModels", "Path", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
