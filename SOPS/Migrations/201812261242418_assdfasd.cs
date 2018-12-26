namespace SOPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assdfasd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyStatistics",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        RegistredProducts = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Date);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CompanyStatistics");
        }
    }
}
