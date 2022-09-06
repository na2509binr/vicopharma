namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateReviewKols : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReviewKols",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Image = c.String(maxLength: 500),
                        VideoLink = c.String(nullable: false),
                        Body = c.String(),
                        Active = c.Boolean(nullable: false),
                        Sort = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.ArticleCategories", "TypePost", c => c.Int(nullable: false));
            AddColumn("dbo.ProductCategories", "TypeProduct", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewKols", "ProductId", "dbo.Products");
            DropIndex("dbo.ReviewKols", new[] { "ProductId" });
            DropColumn("dbo.ProductCategories", "TypeProduct");
            DropColumn("dbo.ArticleCategories", "TypePost");
            DropTable("dbo.ReviewKols");
        }
    }
}
