namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WatchedProducts", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.WatchedProducts", new[] { "ApplicationUserId" });
            DropPrimaryKey("dbo.WatchedProducts");
            AlterColumn("dbo.WatchedProducts", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.WatchedProducts", new[] { "ApplicationUserId", "ProductId" });
            CreateIndex("dbo.WatchedProducts", "ApplicationUserId");
            AddForeignKey("dbo.WatchedProducts", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.WatchedProducts", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WatchedProducts", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.WatchedProducts", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.WatchedProducts", new[] { "ApplicationUserId" });
            DropPrimaryKey("dbo.WatchedProducts");
            AlterColumn("dbo.WatchedProducts", "ApplicationUserId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.WatchedProducts", "Id");
            CreateIndex("dbo.WatchedProducts", "ApplicationUserId");
            AddForeignKey("dbo.WatchedProducts", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
