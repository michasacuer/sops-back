namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasda : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyReports", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.ProductRatings", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductRatings", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CompanyReports", new[] { "CompanyId" });
            DropIndex("dbo.ProductRatings", new[] { "ProductId" });
            DropIndex("dbo.ProductRatings", new[] { "User_Id" });
            DropTable("dbo.CompanyReports");
            DropTable("dbo.ProductRatings");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ProductRatings", "User_Id");
            CreateIndex("dbo.ProductRatings", "ProductId");
            CreateIndex("dbo.CompanyReports", "CompanyId");
            AddForeignKey("dbo.ProductRatings", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ProductRatings", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyReports", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
