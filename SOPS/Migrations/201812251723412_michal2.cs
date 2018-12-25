namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class michal2 : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WatchedProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.WatchedProducts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.WatchedProducts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.WatchedProducts", new[] { "ProductId" });
            DropTable("dbo.WatchedProducts");
        }
    }
}
