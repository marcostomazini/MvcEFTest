namespace MvcEFTest.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Manufacturer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Phone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AndroidVersion = c.String(),
                        Manufacturer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Manufacturer", t => t.Manufacturer_Id)
                .Index(t => t.Manufacturer_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Income = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPhone",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Phone_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Phone_Id })
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Phone", t => t.Phone_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Phone_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPhone", "Phone_Id", "dbo.Phone");
            DropForeignKey("dbo.UserPhone", "User_Id", "dbo.User");
            DropForeignKey("dbo.Phone", "Manufacturer_Id", "dbo.Manufacturer");
            DropIndex("dbo.UserPhone", new[] { "Phone_Id" });
            DropIndex("dbo.UserPhone", new[] { "User_Id" });
            DropIndex("dbo.Phone", new[] { "Manufacturer_Id" });
            DropTable("dbo.UserPhone");
            DropTable("dbo.User");
            DropTable("dbo.Phone");
            DropTable("dbo.Manufacturer");
        }
    }
}
