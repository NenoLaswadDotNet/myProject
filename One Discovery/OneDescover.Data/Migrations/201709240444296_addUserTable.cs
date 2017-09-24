namespace OneDescover.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 150),
                        LastName = c.String(nullable: false, maxLength: 150),
                        Age = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 150),
                        Phone = c.String(nullable: false),
                        AddressLine = c.String(maxLength: 150),
                        UnitOrApartmentNumber = c.String(maxLength: 50),
                        City = c.String(maxLength: 100),
                        State = c.String(maxLength: 50),
                        ZipCode = c.String(maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 64),
                        Salt = c.String(),
                        IsLocked = c.Boolean(nullable: false),
                        LastLockedDateTime = c.DateTime(),
                        FailedAttempts = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_UserID })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserID, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleUsers", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropIndex("dbo.RoleUsers", new[] { "User_UserID" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}
