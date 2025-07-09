namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editdb1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Wards", "ShipFee", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Wards", "ShipFee", c => c.String());
        }
    }
}
