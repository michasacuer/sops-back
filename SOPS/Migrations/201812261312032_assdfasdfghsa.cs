namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assdfasdfghsa : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CompanyStatistics");
            AddColumn("dbo.CompanyStatistics", "CompanyId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.CompanyStatistics", new[] { "Date", "CompanyId" });
            CreateIndex("dbo.CompanyStatistics", "CompanyId");
            AddForeignKey("dbo.CompanyStatistics", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyStatistics", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyStatistics", new[] { "CompanyId" });
            DropPrimaryKey("dbo.CompanyStatistics");
            DropColumn("dbo.CompanyStatistics", "CompanyId");
            AddPrimaryKey("dbo.CompanyStatistics", "Date");
        }
    }
}
