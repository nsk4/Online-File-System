using Microsoft.Ajax.Utilities;
using OnlineFileSystem.Controllers;

namespace OnlineFileSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineFileSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OnlineFileSystem.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            // Init of user roles
            context.UserRoles.AddOrUpdate(p => p.UserRoleId,
                new UserRole
                {
                    Role = "Admin"
                },
                new UserRole
                {
                    Role = "User"
                },
                new UserRole
                {
                    Role = "Unconfirmed"
                }
            );


            // init of sample files
            context.Files.AddOrUpdate(p => p.FileId,
                new File
                {
                    Name = "test1.txt",
                    Sharing = 1,
                    Link = "123214234324",
                    FileType = "txt",
                    Size = 3000,
                    DateCreated = new DateTime(2016, 12, 10),
                    DateModified = new DateTime(2016, 12, 10),
                    Content = new byte[] { 0x0,0x22,0x9,0x1 }
                },
                new File
                {
                    Name = "test2.jpg",
                    Sharing = 0,
                    FileType = "jpg",
                    Size = 552384000,
                    DateCreated = new DateTime(2016, 12, 10),
                    DateModified = new DateTime(2016, 12, 11),
                    Content = new byte[] { 0x1, 0x1, 0x2, 0xFF }
                },
                new File
                {
                    Name = "test3",
                    Sharing = 0,
                    Link = "23432234",
                    FileType = "",
                    Size = 552384000,
                    DateCreated = new DateTime(2016, 12, 9),
                    DateModified = new DateTime(2016, 12, 10),
                    Content = new byte[] { 0x10, 0xA, 0x9, 0x2 }
                }
            );

            // init of folders
            context.Folders.AddOrUpdate(p => p.FolderId,
                new Folder
                {
                    Name = "folder1",
                    DateCreated = new DateTime(2015, 12, 20),
                    DateModified = new DateTime(2016, 12, 20),
                    Files = (from files in context.Files where files.Name == "test1.txt" || files.Name == "test2.txt" select files).ToArray(),
                    Folders = (from folders in context.Folders where folders.Name == "folder3" select folders).ToArray(),
                },
                new Folder
                {
                    Name = "folder2",
                    DateCreated = new DateTime(2015, 12, 21),
                    DateModified = new DateTime(2016, 12, 22),
                    Files = (from files in context.Files where files.Name == "test3.txt" select files).ToArray(),
                },
                new Folder
                {
                    Name = "folder3",
                    DateCreated = new DateTime(2015, 12, 23),
                    DateModified = new DateTime(2016, 12, 24)
                }
            );

            // init of users
            context.Users.AddOrUpdate(p => p.UserId,
                new User
                {
                    Username = "TestUser1",
                    Password = "test",
                    Email = "nejcsk@hotmail.com",
                    Role = context.UserRoles.Single(s=>s.Role=="User"),
                    DateCreated = new DateTime(2014, 10, 20),
                    DateModified = new DateTime(2014, 10, 20),
                    LastLogin = new DateTime(2015, 11, 21),
                    Folders = (from folders in context.Folders where folders.Name == "folder1" select folders).ToArray(),
                },
                new User
                {
                    Username = "TestUser2",
                    Password = "test",
                    Email = "s.k.nejc@gmail.com",
                    Role = context.UserRoles.Single(s => s.Role == "Unconfirmed"),
                    DateCreated = new DateTime(2015, 10, 20),
                    DateModified = new DateTime(2015, 10, 20),
                    LastLogin = new DateTime(2015, 11, 21),
                    Folders = (from folders in context.Folders where folders.Name == "folder2" select folders).ToArray(),
                }
            );
        }
    }
}
