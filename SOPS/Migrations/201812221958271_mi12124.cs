namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mi12124 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CountryOfOrigin", c => c.String());
            AddColumn("dbo.Products", "SuggestedPrice", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "SuggestedPrice");
            DropColumn("dbo.Products", "CountryOfOrigin");
        }
    }
}
