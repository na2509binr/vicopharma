namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixtableOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DiscountAmount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Orders", "DiscountPercent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DiscountPercent", c => c.Single(nullable: false));
            AlterColumn("dbo.Orders", "DiscountAmount", c => c.Int(nullable: false));
        }
    }
}
