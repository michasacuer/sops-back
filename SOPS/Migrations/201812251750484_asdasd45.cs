namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasd45 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.WatchedProducts", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.WatchedProducts", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.WatchedProducts", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.WatchedProducts", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}
