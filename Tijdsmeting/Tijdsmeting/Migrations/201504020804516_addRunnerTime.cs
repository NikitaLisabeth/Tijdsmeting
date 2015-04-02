namespace Tijdsmeting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRunnerTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Runners", "Checkpoint1", c => c.String());
            AddColumn("dbo.Runners", "Checkpoint2", c => c.String());
            AddColumn("dbo.Runners", "Finish", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Runners", "Finish");
            DropColumn("dbo.Runners", "Checkpoint2");
            DropColumn("dbo.Runners", "Checkpoint1");
        }
    }
}
