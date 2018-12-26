namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductRatings", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductRatings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ProductRatings", new[] { "UserId" });
            DropIndex("dbo.ProductRatings", new[] { "ProductId" });
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
            
            AddColumn("dbo.ProductRatings", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.ProductRatings", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductRatings", "User_Id");
            AddForeignKey("dbo.ProductRatings", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "Surname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropForeignKey("dbo.ProductRatings", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductRatingProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductRatingProducts", "ProductRating_Id", "dbo.ProductRatings");
            DropIndex("dbo.ProductRatingProducts", new[] { "Product_Id" });
            DropIndex("dbo.ProductRatingProducts", new[] { "ProductRating_Id" });
            DropIndex("dbo.ProductRatings", new[] { "User_Id" });
            AlterColumn("dbo.ProductRatings", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.ProductRatings", "User_Id");
            DropTable("dbo.ProductRatingProducts");
            CreateIndex("dbo.ProductRatings", "ProductId");
            CreateIndex("dbo.ProductRatings", "UserId");
            AddForeignKey("dbo.ProductRatings", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ProductRatings", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
