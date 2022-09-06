namespace Ephyta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllShoppingCart : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Sort = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Prefix = c.String(maxLength: 20),
                        ShipFee = c.Int(nullable: false),
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
                        Sort = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Prefix = c.String(maxLength: 20),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.DistrictId);
            
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
                        DiscountAmount = c.Int(nullable: false),
                        DiscountPercent = c.Single(nullable: false),
                        OrderMemberId = c.Int(),
                        TotalPropertyPrice = c.Decimal(precision: 18, scale: 2),
                        CityId = c.Int(),
                        DistrictId = c.Int(),
                        WardId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.Wards", t => t.WardId)
                .Index(t => t.CityId)
                .Index(t => t.DistrictId)
                .Index(t => t.WardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Orders", "WardId", "dbo.Wards");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Orders", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Wards", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Districts", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropIndex("dbo.Orders", new[] { "WardId" });
            DropIndex("dbo.Orders", new[] { "DistrictId" });
            DropIndex("dbo.Orders", new[] { "CityId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Wards", new[] { "DistrictId" });
            DropIndex("dbo.Districts", new[] { "CityId" });
            DropIndex("dbo.Carts", new[] { "ProductId" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Wards");
            DropTable("dbo.Districts");
            DropTable("dbo.Cities");
            DropTable("dbo.Carts");
        }
    }
}
