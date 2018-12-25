namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Kind = c.String(),
                        AddressStreet = c.String(),
                        AddressZipCode = c.String(),
                        AddressCity = c.String(),
                        Email = c.String(),
                        NIP = c.String(),
                        REGON = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.WatchedProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Barcode = c.String(),
                        Description = c.String(),
                        CountryOfOrigin = c.String(nullable: false),
                        SuggestedPrice = c.Decimal(nullable: false, storeType: "money"),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.ExistingProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpirationDate = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.QRs",
                c => new
                    {
                        ExistingProductId = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        Content = c.Binary(),
                    })
                .PrimaryKey(t => t.ExistingProductId)
                .ForeignKey("dbo.ExistingProducts", t => t.ExistingProductId)
                .Index(t => t.ExistingProductId);
            
            CreateTable(
                "dbo.ProductRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Single(nullable: false),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ProductRatingProducts",
                c => new
                    {
                        ProductRating_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductRating_Id, t.Product_Id })
                .ForeignKey("dbo.ProductRatings", t => t.ProductRating_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.ProductRating_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.WatchedProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductRatings", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.WatchedProducts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductRatingProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductRatingProducts", "ProductRating_Id", "dbo.ProductRatings");
            DropForeignKey("dbo.QRs", "ExistingProductId", "dbo.ExistingProducts");
            DropForeignKey("dbo.ExistingProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUsers", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.CompanyReports", "CompanyId", "dbo.Companies");
            DropIndex("dbo.ProductRatingProducts", new[] { "Product_Id" });
            DropIndex("dbo.ProductRatingProducts", new[] { "ProductRating_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProductRatings", new[] { "User_Id" });
            DropIndex("dbo.QRs", new[] { "ExistingProductId" });
            DropIndex("dbo.ExistingProducts", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "CompanyId" });
            DropIndex("dbo.WatchedProducts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.WatchedProducts", new[] { "ProductId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Company_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CompanyReports", new[] { "CompanyId" });
            DropTable("dbo.ProductRatingProducts");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProductRatings");
            DropTable("dbo.QRs");
            DropTable("dbo.ExistingProducts");
            DropTable("dbo.Products");
            DropTable("dbo.WatchedProducts");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CompanyReports");
            DropTable("dbo.Companies");
        }
    }
}
