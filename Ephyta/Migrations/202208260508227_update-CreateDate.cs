namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCreateDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "CreateDate");
        }
    }
}
