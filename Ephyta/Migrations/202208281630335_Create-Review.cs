namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateReview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 700),
                        Image = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "ProductId", "dbo.Products");
            DropIndex("dbo.Reviews", new[] { "ProductId" });
            DropTable("dbo.Reviews");
        }
    }
}
