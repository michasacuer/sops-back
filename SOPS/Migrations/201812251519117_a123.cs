namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a123 : DbMigration
    {
        public override void Up()
        {
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
            DropForeignKey("dbo.ProductRatings", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductRatingProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductRatingProducts", "ProductRating_Id", "dbo.ProductRatings");
            DropForeignKey("dbo.CompanyReports", "CompanyId", "dbo.Companies");
            DropIndex("dbo.ProductRatingProducts", new[] { "Product_Id" });
            DropIndex("dbo.ProductRatingProducts", new[] { "ProductRating_Id" });
            DropIndex("dbo.ProductRatings", new[] { "User_Id" });
            DropIndex("dbo.CompanyReports", new[] { "CompanyId" });
            DropTable("dbo.ProductRatingProducts");
            DropTable("dbo.ProductRatings");
            DropTable("dbo.CompanyReports");
        }
    }
}
