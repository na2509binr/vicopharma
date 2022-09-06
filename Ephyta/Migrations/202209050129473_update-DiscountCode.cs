namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDiscountCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountCodes", "Discount", c => c.Int(nullable: false));
            AddColumn("dbo.DiscountCodes", "TypeDiscount", c => c.Int(nullable: false));
            AddColumn("dbo.DiscountCodes", "ExpDay", c => c.String());
            AlterColumn("dbo.DiscountCodes", "Fullname", c => c.String(maxLength: 50));
            AlterColumn("dbo.DiscountCodes", "Price", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DiscountCodes", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.DiscountCodes", "Fullname", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.DiscountCodes", "ExpDay");
            DropColumn("dbo.DiscountCodes", "TypeDiscount");
            DropColumn("dbo.DiscountCodes", "Discount");
        }
    }
}
