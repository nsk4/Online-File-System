namespace OnlineFileSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Role_UserRoleId", "dbo.UserRoles");
            DropIndex("dbo.Users", new[] { "Role_UserRoleId" });
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        UserAccountId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        LastLogin = c.DateTime(nullable: false),
                        Role_UserRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserAccountId)
                .ForeignKey("dbo.UserRoles", t => t.Role_UserRoleId, cascadeDelete: true)
                .Index(t => t.Role_UserRoleId);
            
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        LastLogin = c.DateTime(nullable: false),
                        Role_UserRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropForeignKey("dbo.UserAccounts", "Role_UserRoleId", "dbo.UserRoles");
            DropIndex("dbo.UserAccounts", new[] { "Role_UserRoleId" });
            DropTable("dbo.UserAccounts");
            CreateIndex("dbo.Users", "Role_UserRoleId");
            AddForeignKey("dbo.Users", "Role_UserRoleId", "dbo.UserRoles", "UserRoleId", cascadeDelete: true);
        }
    }
}
