﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Controllers
{
	[AuthorizationFilter(Utility.AccountType.Admin, Utility.AccountType.User)]
	public class MainController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


		// GET: Main
		public ActionResult Index()
		{
			/*
			if (Session["user"] == null) return View("Error");
			UserAccount ua = (UserAccount)Session["user"];
			//UserAccount ua = db.UserAccounts.Single(u => u.Username == "TestUser1");
			List<Folder> folderList =
				(from item in db.Folders.ToList() where item.OwnerUserAccount == ua select item).ToList();
			ViewBag.folderList = folderList;
			List<File> fileList =
				(from item in db.Files.ToList() where folderList.Contains(item.ParentFolder) select item).ToList();
			ViewBag.fileList = fileList;
			*/
			return RedirectToAction("OpenFolder", "Main");
		}


		
		public ActionResult OpenFolder(int? folderId)
        {
            //if (userId == null) return View("Error");
            //UserAccount ua = db.UserAccounts.Find(userId);
            //if (ua == null) return View("Error");

			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
            Folder currentFolder = null;
			if (folderId != null)
            {
                currentFolder = db.Folders.Find(folderId);
                if (currentFolder == null || currentFolder.OwnerUserAccount != ua) return View("Error");
            }

            List<Folder> folderList = (db.Folders.ToList().Where(item => item.OwnerUserAccount == ua && item.ParentFolder == currentFolder)).ToList();
            List<File> fileList = (db.Files.ToList().Where(item => item.ParentFolder == currentFolder && item.OwnerUserAccount == ua)).ToList();

            ViewBag.currentFolder = currentFolder;
            ViewBag.folderList = folderList;
            ViewBag.fileList = fileList;
            //ViewBag.userId = ua.UserAccountId;

            return View("Index");
        }

		[ValidateAntiForgeryToken]
        [HttpPost]
		public ActionResult CreateFolder(int? currentFolderId, string newFolderName)
        {
            if (string.IsNullOrWhiteSpace(newFolderName)) return View("Error");

			//UserAccount ua = db.UserAccounts.Find(Session["user"]);
			//if (ua == null) return View("Error");
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			Folder currentFolder = null;
            if (currentFolderId != null)
            {
                currentFolder = db.Folders.Find(currentFolderId);
                if (currentFolder == null || currentFolder.OwnerUserAccount != ua) return View("Error");
            }
	        
			Folder f = new Folder
            {
                OwnerUserAccount = ua,
                ParentFolder = currentFolder,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Name = newFolderName
            };
            db.Folders.Add(f);
            db.SaveChanges();
            
            return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId});
        }

		[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RenameFolder(int? currentFolderId, string newFolderName)
        {
            if (string.IsNullOrWhiteSpace(newFolderName)) return View("Error");

            // if (userId == null) return View("Error");
            //UserAccount ua = db.UserAccounts.Find(Session["user"]);
            //if (ua == null) return View("Error");

            if (currentFolderId == null) return View("Error");
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			Folder currentFolder = db.Folders.Find(currentFolderId);
			if (currentFolder == null || currentFolder.OwnerUserAccount != ua) return View("Error");

            currentFolder.Name = newFolderName;
            currentFolder.DateModified = DateTime.Now;
            //db.Entry(currentFolder).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder.FolderId});
        }

		[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteFolder(int? currentFolderId, bool? confirmFolderDelete)
        {
            if (confirmFolderDelete == null) // user did not confirm folder delete
            {
                return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolderId});
            }

            //if (userId == null) return View("Error");
            //UserAccount ua = db.UserAccounts.Find(userId);
            //if (ua == null) return View("Error");

            if (currentFolderId == null) return View("Error");
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
            Folder currentFolder = db.Folders.Find(currentFolderId);
			if (currentFolder == null || currentFolder.OwnerUserAccount != ua) return View("Error");

            Folder parentFolder = currentFolder.ParentFolder;

            // remove all files in a folder
            List<File> fileList = (db.Files.ToList().Where(item => item.ParentFolder == currentFolder && item.OwnerUserAccount == ua)).ToList();
            List<FileContent> fileContentList = (db.FileContents.ToList().Where(item => fileList.Select(x => x.Content).Contains(item))).ToList();

            db.Files.RemoveRange(fileList);
            db.FileContents.RemoveRange(fileContentList);
            db.Folders.Remove(currentFolder); // delete folder
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = parentFolder?.FolderId});
        }

		[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UploadFile(int? currentFolderId, string newFileName, HttpPostedFileBase file)
        {
            
            if (string.IsNullOrWhiteSpace(newFileName) ||
                file == null || 
                file.ContentLength <= 0) return View("Error");

            string fileName = file.FileName;
            System.IO.MemoryStream target = new System.IO.MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] fileData = target.ToArray();


			//if (userId == null) return View("Error");
			//UserAccount ua = db.UserAccounts.Find(userId);
			//if (ua == null) return View("Error");

			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			Folder currentFolder = null;
            if (currentFolderId != null)
            {
                currentFolder = db.Folders.Find(currentFolderId);
                if (currentFolder == null || currentFolder.OwnerUserAccount != ua) return View("Error");
            }

            FileContent fc = new FileContent();
            fc.Data = fileData;

            string fileType = "/";
            string[] possibleTypes = fileName.Split('.');
            if (possibleTypes.Length > 1) fileType = possibleTypes.Last();

			string link = null;
			do
			{
				link = (ua.Username + newFileName + DateTime.Now.ToString("U") + (new Random()).Next(0, 1000).ToString()).GetHashCode().ToString();
			} while (db.Files.Any(x => x.Link == link));

			File f = new File
            {
                OwnerUserAccount = ua,
                Content = fc,
                Size = fileData.Length,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                FileType = fileType,
                Link = link,
                Name = newFileName,
                ParentFolder = currentFolder,
                Sharing = 0
            };

            db.FileContents.Add(fc);
            db.Files.Add(f);
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId });
        }

		public ActionResult DownloadFile(int? fileId)
        {
            //if (userId == null) return View("Error");
            //UserAccount ua = db.UserAccounts.Find(userId);
            //if (ua == null) return View("Error");

            if (fileId == null) return View("Error");
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			File f = db.Files.Find(fileId);
            if (f == null || f.OwnerUserAccount != ua) return View("Error");

			db.Entry(f).Reference(c => c.Content).Load();
			//FileContent fc = db.Files.Include(x => x.Content).First(x => x.FileId == fileId).Content; // damn lazy loading is lazy so make it eager
            //if (fc == null || fc.Data.Length <= 0) return View("Error");
	        FileContent fc = f.Content;

            string fileName = f.Name;
            if (f.FileType != "/" && f.Name.Split('.').Last() != f.FileType) fileName += "." + f.FileType;

            return File(fc.Data, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

		public ActionResult DeleteFile(int? currentFolderId, int? fileId)
        {
			//if (userId == null) return View("Error");
			//UserAccount ua = db.UserAccounts.Find(userId);
			//if (ua == null) return View("Error");

			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			Folder currentFolder = null;
            if (currentFolderId != null)
            {
                currentFolder = db.Folders.Find(currentFolderId);
                if (currentFolder == null || currentFolder.OwnerUserAccount != ua) return View("Error");
            }

            if (fileId == null) return View("Error");
            File f = db.Files.Find(fileId);
            if (f == null || f.OwnerUserAccount != ua) return View("Error");

			db.Entry(f).Reference(c => c.Content).Load();
	        FileContent fc = f.Content;
			//FileContent fc = db.Files.Include(x => x.Content).FirstOrDefault(x => x.FileId == fileId).Content; // damn lazy loading is lazy so make it eager
			// if (fc == null || fc.Data.Length <= 0) return View("Error");

			db.Files.Remove(f);
            db.FileContents.Remove(fc);
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId });
        }

		public ActionResult ShareFile(int? currentFolderId, int? fileId)
        {
			//if (userId == null) return View("Error");
			//UserAccount ua = db.UserAccounts.Find(userId);
			//if (ua == null) return View("Error");

			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			Folder currentFolder = null;
            if (currentFolderId != null)
            {
                currentFolder = db.Folders.Find(currentFolderId);
                if (currentFolder == null || currentFolder.OwnerUserAccount != ua) return View("Error");
            }

            if (fileId == null) return View("Error");
            File f = db.Files.Find(fileId);
            if (f == null || f.OwnerUserAccount != ua) return View("Error");
			db.Entry(f).Reference(c => c.Content).Load();

			f.Sharing = 1;
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId });
        }


		public ActionResult StopShareFile(int? currentFolderId, int? fileId)
		{
			//if (userId == null) return View("Error");
			//UserAccount ua = db.UserAccounts.Find(userId);
			//if (ua == null) return View("Error");

			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			Folder currentFolder = null;
			if (currentFolderId != null)
			{
				currentFolder = db.Folders.Find(currentFolderId);
				if (currentFolder == null || currentFolder.OwnerUserAccount != ua) return View("Error");
			}

			if (fileId == null) return View("Error");
			File f = db.Files.Find(fileId);
			if (f == null || f.OwnerUserAccount != ua) return View("Error");
			db.Entry(f).Reference(c => c.Content).Load();

			f.Sharing = 0;
			db.SaveChanges();

			return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId });
		}

		[AllowAnonymous]
		public ActionResult DownloadSharedFile(string url)
	    {
			if(url == null) return View("Error");
			File f = db.Files.FirstOrDefault(s => s.Link == url);
			if (f == null || f.Sharing == 0) return View("Error");

			db.Entry(f).Reference(c => c.Content).Load();
			FileContent fc = f.Content;

			string fileName = f.Name;
			if (f.FileType != "/" && f.Name.Split('.').Last() != f.FileType) fileName += "." + f.FileType;

			return File(fc.Data, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
		}

	}
}