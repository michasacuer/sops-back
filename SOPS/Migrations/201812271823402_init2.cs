namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductRatings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ProductRatings", new[] { "UserId" });
            DropPrimaryKey("dbo.ProductRatings");
            CreateTable(
                "dbo.ShortUrls",
                c => new
                    {
                        Url = c.String(nullable: false, maxLength: 128),
                        DestinationUrl = c.String(),
                        Added = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Url);
            
            AlterColumn("dbo.ProductRatings", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ProductRatings", new[] { "UserId", "ProductId" });
            CreateIndex("dbo.ProductRatings", "UserId");
            AddForeignKey("dbo.ProductRatings", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.ProductRatings", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductRatings", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.ProductRatings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ProductRatings", new[] { "UserId" });
            DropPrimaryKey("dbo.ProductRatings");
            AlterColumn("dbo.ProductRatings", "UserId", c => c.String(maxLength: 128));
            DropTable("dbo.ShortUrls");
            AddPrimaryKey("dbo.ProductRatings", "Id");
            CreateIndex("dbo.ProductRatings", "UserId");
            AddForeignKey("dbo.ProductRatings", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
