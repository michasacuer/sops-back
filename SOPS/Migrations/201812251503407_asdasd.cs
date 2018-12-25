namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasd : DbMigration
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
            
            AlterColumn("dbo.Products", "CountryOfOrigin", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyReports", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyReports", new[] { "CompanyId" });
            AlterColumn("dbo.Products", "CountryOfOrigin", c => c.String());
            DropTable("dbo.CompanyReports");
        }
    }
}
