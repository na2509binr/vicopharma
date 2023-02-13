namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TikTokconfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConfigSites", "TikTok", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConfigSites", "TikTok");
        }
    }
}
