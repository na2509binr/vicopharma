namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateConfigArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Col", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConfigSites", "VideoIntro", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConfigSites", "VideoIntro");
            DropColumn("dbo.Articles", "Col");
        }
    }
}
