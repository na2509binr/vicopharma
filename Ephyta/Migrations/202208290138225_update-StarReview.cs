namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateStarReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "ImageAvata", c => c.String());
            AddColumn("dbo.Reviews", "ListImage", c => c.String());
            AddColumn("dbo.Reviews", "StarReview", c => c.Int(nullable: false));
            DropColumn("dbo.Reviews", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "Image", c => c.String());
            DropColumn("dbo.Reviews", "StarReview");
            DropColumn("dbo.Reviews", "ListImage");
            DropColumn("dbo.Reviews", "ImageAvata");
        }
    }
}
