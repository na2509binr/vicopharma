namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editdb2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cities", "CitySort", c => c.Int(nullable: false));
            AddColumn("dbo.Cities", "CityActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Wards", "WardSort", c => c.Int(nullable: false));
            AddColumn("dbo.Wards", "WardActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Cities", "Sort");
            DropColumn("dbo.Cities", "Active");
            DropColumn("dbo.Wards", "Sort");
            DropColumn("dbo.Wards", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wards", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Wards", "Sort", c => c.Int(nullable: false));
            AddColumn("dbo.Cities", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cities", "Sort", c => c.Int(nullable: false));
            DropColumn("dbo.Wards", "WardActive");
            DropColumn("dbo.Wards", "WardSort");
            DropColumn("dbo.Cities", "CityActive");
            DropColumn("dbo.Cities", "CitySort");
        }
    }
}
