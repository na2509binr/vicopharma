namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixRegister_Product : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Registers", "Product", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Registers", "Product", c => c.String());
        }
    }
}
