namespace Tijdsmeting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initiateDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Runners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Firstname = c.String(),
                        Barcode = c.String(),
                        RFID = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Runners");
        }
    }
}
