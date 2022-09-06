namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodeExpDay : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DiscountCodes", "ExpDay", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DiscountCodes", "ExpDay", c => c.String());
        }
    }
}
