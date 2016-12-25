using OnlineFileSystem.Models;

namespace OnlineFileSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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


            

            UserRole roleAdmin = new UserRole();
            roleAdmin.Role = "Admin";
            UserRole roleUser = new UserRole();
            roleUser.Role = "User";
            UserRole roleUnconfirmed = new UserRole();
            roleUnconfirmed.Role = "Unconfirmed";

            
            FileContent fileC1 = new FileContent();
            fileC1.Data = new byte[] {8, 4, 5, 5};
            FileContent fileC2 = new FileContent();
            fileC2.Data = new byte[] {8, 4, 5, 5};
            FileContent fileC3 = new FileContent();
            fileC3.Data = new byte[] {3, 4, 9, 1};





            UserAccount user1 = new UserAccount();
            user1.Username = "TestUser1";
            user1.Password = "test";
            user1.PasswordSalt = "nippon";
            user1.Email = "nejcsk@hotmail.com";
            user1.Role = roleUser;
            user1.DateCreated = new DateTime(2014, 10, 20);
            user1.DateModified = new DateTime(2014, 10, 20);
            user1.LastLogin = new DateTime(2015, 11, 21);

            UserAccount user2 = new UserAccount();
            user2.Username = "TestUser2";
            user2.Password = "test";
            user2.PasswordSalt = "nippon";
            user2.Email = "s.k.nejc@gmail.com";
            user2.Role = roleUnconfirmed;
            user2.DateCreated = new DateTime(2015, 10, 20);
            user2.DateModified = new DateTime(2015, 10, 20);
            user2.LastLogin = new DateTime(2015, 11, 21);


            Folder folder1 = new Folder();
            folder1.Name = "folder1";
            folder1.DateCreated = new DateTime(2015, 12, 20);
            folder1.DateModified = new DateTime(2016, 12, 20);
            folder1.OwnerUserAccount = user1;

            Folder folder2 = new Folder();
            folder2.Name = "folder2";
            folder2.DateCreated = new DateTime(2015, 12, 21);
            folder2.DateModified = new DateTime(2016, 12, 22);
            folder2.OwnerUserAccount = user2;

            Folder folder3 = new Folder();
            folder3.Name = "folder3";
            folder3.DateCreated = new DateTime(2015, 12, 23);
            folder3.DateModified = new DateTime(2016, 12, 24);
            folder3.ParentFolder = folder1;
            folder3.OwnerUserAccount = user1;



            File file1 = new File();
            file1.Name = "test1.txt";
            file1.Sharing = 1;
            file1.Link = "123214234324";
            file1.FileType = "txt";
            file1.Size = 3000;
            file1.DateCreated = new DateTime(2016, 12, 10);
            file1.DateModified = new DateTime(2016, 12, 10);
            file1.Content = fileC1;
            file1.ParentFolder = folder1;

            File file2 = new File();
            file2.Name = "test2.jpg";
            file2.Sharing = 0;
            file2.FileType = "jpg";
            file2.Size = 552384000;
            file2.DateCreated = new DateTime(2016, 12, 10);
            file2.DateModified = new DateTime(2016, 12, 11);
            file2.Content = fileC2;
            file2.ParentFolder = folder1;

            File file3 = new File();
            file3.Name = "test3";
            file3.Sharing = 0;
            file3.FileType = "/";
            file3.Link = "23432234";
            file3.Size = 552384000;
            file3.DateCreated = new DateTime(2016, 12, 9);
            file3.DateModified = new DateTime(2016, 12, 10);
            file3.Content = fileC3;
            file3.ParentFolder = folder2;

            

            context.UserRoles.AddOrUpdate(roleAdmin, roleUser, roleUnconfirmed);
            context.UserAccounts.AddOrUpdate(user1, user2);
            context.Folders.AddOrUpdate(folder1, folder2, folder3);
            context.FileContents.AddOrUpdate(fileC1, fileC2, fileC2);
            context.Files.AddOrUpdate(file1, file2, file3);
            
            


            


            /*
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
            
            // init binary data
            context.FileContents.AddOrUpdate(p => p.FileContentId,
                new FileContent
                {
                    Data = new byte[] { 8, 4, 5, 5 }
                },
                new FileContent
                {
                    Data = new byte[] { 8, 4, 5, 5 }
                },
                new FileContent
                {
                    Data = new byte[] { 3, 4, 9, 1 }
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
                    Content = (from contents in context.FileContents where contents.FileContentId == 1 select contents).First()
                },
                new File
                {
                    Name = "test2.jpg",
                    Sharing = 0,
                    FileType = "jpg",
                    Size = 552384000,
                    DateCreated = new DateTime(2016, 12, 10),
                    DateModified = new DateTime(2016, 12, 11),
                    Content = (from contents in context.FileContents where contents.FileContentId == 2 select contents).First()
                },
                new File
                {
                    Name = "test3",
                    Sharing = 0,
                    Link = "23432234",
                    FileType = "/",
                    Size = 552384000,
                    DateCreated = new DateTime(2016, 12, 9),
                    DateModified = new DateTime(2016, 12, 10),
                    Content = (from contents in context.FileContents where contents.FileContentId == 3 select contents).First()
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
            context.UserAccounts.AddOrUpdate(p => p.UserAccountId,
                new UserAccount
                {
                    Username = "TestUser1",
                    Password = "test",
                    Email = "nejcsk@hotmail.com",
                    Role = (from userRoles in context.UserRoles where userRoles.Role == "User" select userRoles).First(),
                    DateCreated = new DateTime(2014, 10, 20),
                    DateModified = new DateTime(2014, 10, 20),
                    LastLogin = new DateTime(2015, 11, 21),
                    Folders = (from folders in context.Folders where folders.Name == "folder1" select folders).ToArray(),
                },
                new UserAccount
                {
                    Username = "TestUser2",
                    Password = "test",
                    Email = "s.k.nejc@gmail.com",
                    Role = (from userRoles in context.UserRoles where userRoles.Role == "Unconfirmed" select userRoles).First(),
                    DateCreated = new DateTime(2015, 10, 20),
                    DateModified = new DateTime(2015, 10, 20),
                    LastLogin = new DateTime(2015, 11, 21),
                    Folders = (from folders in context.Folders where folders.Name == "folder2" select folders).ToArray(),
                }
            );
            */
        }
    }
}
