namespace MvcEFTest.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class DateTime2Config : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manufacturer", "HeadquartersLocation", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturer", "HeadquartersLocation");
        }
    }
}
