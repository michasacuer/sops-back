namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mi2131234 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyReports", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyReports", new[] { "CompanyId" });
            DropTable("dbo.CompanyReports");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CompanyReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.CompanyReports", "CompanyId");
            AddForeignKey("dbo.CompanyReports", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
