namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 60),
                        Active = c.Boolean(nullable: false),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                        Url = c.String(maxLength: 500),
                        CategorySort = c.Int(nullable: false),
                        CategoryActive = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        ShowHome = c.Boolean(nullable: false),
                        ShowMenu = c.Boolean(nullable: false),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                        Image = c.String(maxLength: 500),
                        CoverImage = c.String(maxLength: 500),
                        TypePost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false, maxLength: 500),
                        Body = c.String(),
                        Image = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        View = c.Int(nullable: false),
                        ArticleCategoryId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Col = c.Boolean(nullable: false),
                        Url = c.String(maxLength: 300),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleCategories", t => t.ArticleCategoryId, cascadeDelete: true)
                .Index(t => t.ArticleCategoryId);
            
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BannerName = c.String(nullable: false, maxLength: 100),
                        Slogan = c.String(maxLength: 100),
                        Image = c.String(maxLength: 500),
                        Active = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Url = c.String(maxLength: 500),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        ProductId = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ListImage = c.String(),
                        Description = c.String(nullable: false),
                        Function = c.String(),
                        Usermanual = c.String(),
                        Intro = c.String(),
                        Use = c.String(maxLength: 2000),
                        Specifications = c.String(maxLength: 100),
                        Ingredient = c.String(),
                        ProductCategoryId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        SaleOff = c.Decimal(precision: 18, scale: 2),
                        Sort = c.Int(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Hot = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                        Url = c.String(maxLength: 500),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Image = c.String(maxLength: 500),
                        Url = c.String(maxLength: 500),
                        Sort = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Home = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                        TypeProduct = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 700),
                        ImageAvata = c.String(),
                        ListImage = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        StarReview = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CitySort = c.Int(nullable: false),
                        CityActive = c.Boolean(nullable: false),
                        Prefix = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Facebook = c.String(maxLength: 500),
                        Linkedin = c.String(maxLength: 500),
                        TikTok = c.String(maxLength: 500),
                        Instagram = c.String(maxLength: 500),
                        Twitter = c.String(maxLength: 500),
                        Youtube = c.String(maxLength: 500),
                        UrlMessenger = c.String(maxLength: 200),
                        LiveChat = c.String(maxLength: 4000),
                        Image = c.String(),
                        ImageFooter = c.String(),
                        GoogleMap = c.String(maxLength: 4000),
                        GoogleAnalytics = c.String(maxLength: 4000),
                        Place = c.String(),
                        Title = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        Hotline = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        InfoFooter = c.String(),
                        VideoIntro = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fullname = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 200),
                        Mobile = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Body = c.String(maxLength: 4000),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiscountCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fullname = c.String(maxLength: 50),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Discount = c.Int(nullable: false),
                        TypeDiscount = c.Int(nullable: false),
                        ExpDay = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Sort = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Prefix = c.String(maxLength: 20),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Wards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        WardSort = c.Int(nullable: false),
                        WardActive = c.Boolean(nullable: false),
                        Prefix = c.String(maxLength: 20),
                        ShipFee = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        District_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Districts", t => t.District_Id)
                .Index(t => t.CityId)
                .Index(t => t.District_Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 700),
                        Image = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Classify = c.String(maxLength: 100),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaDonHang = c.String(maxLength: 50),
                        CreateDate = c.DateTime(nullable: false),
                        Payment = c.Boolean(nullable: false),
                        TypePay = c.Int(nullable: false),
                        Transport = c.Int(nullable: false),
                        TransportDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Viewed = c.Boolean(nullable: false),
                        CustomerInfo_Fullname = c.String(nullable: false, maxLength: 50),
                        CustomerInfo_Address = c.String(nullable: false, maxLength: 200),
                        CustomerInfo_Mobile = c.String(nullable: false, maxLength: 11),
                        CustomerInfo_Email = c.String(maxLength: 50),
                        CustomerInfo_Body = c.String(maxLength: 200),
                        CustomerInfo_IsNewMember = c.Boolean(nullable: false),
                        ThanhToanTruoc = c.Int(nullable: false),
                        ShipFee = c.Int(nullable: false),
                        DiscountCode = c.String(maxLength: 50),
                        DiscountAmount = c.Decimal(precision: 18, scale: 2),
                        DiscountPercent = c.Int(nullable: false),
                        OrderMemberId = c.Int(),
                        TotalPropertyPrice = c.Decimal(precision: 18, scale: 2),
                        CityId = c.Int(),
                        WardId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Wards", t => t.WardId)
                .Index(t => t.CityId)
                .Index(t => t.WardId);
            
            CreateTable(
                "dbo.Registers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fullname = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(nullable: false),
                        Time = c.String(),
                        Product = c.String(nullable: false),
                        Note = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Image = c.String(maxLength: 500),
                        VideoLink = c.String(nullable: false),
                        Body = c.String(),
                        Active = c.Boolean(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Sort = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Orders", "WardId", "dbo.Wards");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Wards", "District_Id", "dbo.Districts");
            DropForeignKey("dbo.Wards", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Districts", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Reviews", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ReviewKols", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.Articles", "ArticleCategoryId", "dbo.ArticleCategories");
            DropIndex("dbo.Orders", new[] { "WardId" });
            DropIndex("dbo.Orders", new[] { "CityId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Wards", new[] { "District_Id" });
            DropIndex("dbo.Wards", new[] { "CityId" });
            DropIndex("dbo.Districts", new[] { "CityId" });
            DropIndex("dbo.Reviews", new[] { "ProductId" });
            DropIndex("dbo.ReviewKols", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropIndex("dbo.Carts", new[] { "ProductId" });
            DropIndex("dbo.Articles", new[] { "ArticleCategoryId" });
            DropTable("dbo.Videos");
            DropTable("dbo.Registers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Wards");
            DropTable("dbo.Districts");
            DropTable("dbo.DiscountCodes");
            DropTable("dbo.Contacts");
            DropTable("dbo.ConfigSites");
            DropTable("dbo.Cities");
            DropTable("dbo.Reviews");
            DropTable("dbo.ReviewKols");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Carts");
            DropTable("dbo.Banners");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleCategories");
            DropTable("dbo.Admins");
        }
    }
}
