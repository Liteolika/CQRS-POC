namespace CQRS_Views.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NetworkDeviceDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Hostname = c.String(),
                        Version = c.Int(nullable: false),
                        IsOnline = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NetworkDeviceDetails");
        }
    }
}
