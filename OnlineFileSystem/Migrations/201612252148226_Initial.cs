namespace OnlineFileSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileContents",
                c => new
                    {
                        FileContentId = c.Int(nullable: false, identity: true),
                        Data = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.FileContentId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Sharing = c.Int(nullable: false),
                        Link = c.String(),
                        FileType = c.String(nullable: false),
                        Size = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        Content_FileContentId = c.Int(nullable: false),
                        ParentFolder_FolderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.FileContents", t => t.Content_FileContentId, cascadeDelete: true)
                .ForeignKey("dbo.Folders", t => t.ParentFolder_FolderId, cascadeDelete: true)
                .Index(t => t.Content_FileContentId)
                .Index(t => t.ParentFolder_FolderId);
            
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        FolderId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        OwnerUserAccount_UserAccountId = c.Int(nullable: false),
                        ParentFolder_FolderId = c.Int(),
                    })
                .PrimaryKey(t => t.FolderId)
                .ForeignKey("dbo.UserAccounts", t => t.OwnerUserAccount_UserAccountId, cascadeDelete: true)
                .ForeignKey("dbo.Folders", t => t.ParentFolder_FolderId)
                .Index(t => t.OwnerUserAccount_UserAccountId)
                .Index(t => t.ParentFolder_FolderId);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        UserAccountId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        LastLogin = c.DateTime(nullable: false),
                        Role_UserRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserAccountId)
                .ForeignKey("dbo.UserRoles", t => t.Role_UserRoleId, cascadeDelete: true)
                .Index(t => t.Role_UserRoleId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserRoleId = c.Int(nullable: false, identity: true),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserRoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Files", "ParentFolder_FolderId", "dbo.Folders");
            DropForeignKey("dbo.Folders", "ParentFolder_FolderId", "dbo.Folders");
            DropForeignKey("dbo.Folders", "OwnerUserAccount_UserAccountId", "dbo.UserAccounts");
            DropForeignKey("dbo.UserAccounts", "Role_UserRoleId", "dbo.UserRoles");
            DropForeignKey("dbo.Files", "Content_FileContentId", "dbo.FileContents");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.UserAccounts", new[] { "Role_UserRoleId" });
            DropIndex("dbo.Folders", new[] { "ParentFolder_FolderId" });
            DropIndex("dbo.Folders", new[] { "OwnerUserAccount_UserAccountId" });
            DropIndex("dbo.Files", new[] { "ParentFolder_FolderId" });
            DropIndex("dbo.Files", new[] { "Content_FileContentId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Folders");
            DropTable("dbo.Files");
            DropTable("dbo.FileContents");
        }
    }
}
