using System;
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
		readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private ApplicationDbContext db = new ApplicationDbContext();


		/// <summary>
		/// Default method that redirects to OpenFolder method
		/// </summary>
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

		/// <summary>
		/// Gets the folders and files for requested folder
		/// </summary>
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
	            if (currentFolder == null || currentFolder.OwnerUserAccount != ua)
	            {
					TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
					return RedirectToAction("Index", "Main");
	            }
            }

            List<Folder> folderList = (db.Folders.ToList().Where(item => item.OwnerUserAccount == ua && item.ParentFolder == currentFolder)).ToList();
            List<File> fileList = (db.Files.ToList().Where(item => item.ParentFolder == currentFolder && item.OwnerUserAccount == ua)).ToList();

            ViewBag.currentFolder = currentFolder;
            ViewBag.folderList = folderList;
            ViewBag.fileList = fileList;
			//ViewBag.userId = ua.UserAccountId;

			ViewBag.Error = TempData["Error"];
			return View("Index");
        }

		/// <summary>
		/// Creates folder in a hierarchy
		/// </summary>
		[ValidateAntiForgeryToken]
        [HttpPost]
		public ActionResult CreateFolder(int? currentFolderId, string newFolderName)
        {
	        if (string.IsNullOrWhiteSpace(newFolderName))
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				return RedirectToAction("Index", "Main");
			}

			//UserAccount ua = db.UserAccounts.Find(Session["user"]);
			//if (ua == null) return View("Error");
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			Folder currentFolder = null;
            if (currentFolderId != null)
            {
                currentFolder = db.Folders.Find(currentFolderId);
	            if (currentFolder == null || currentFolder.OwnerUserAccount != ua)
	            {
					TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
					return RedirectToAction("Index", "Main");
				}
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

		/// <summary>
		/// Renames folder
		/// </summary>
		[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RenameFolder(int? currentFolderId, string newFolderName)
        {
	        if (string.IsNullOrWhiteSpace(newFolderName))
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				return RedirectToAction("Index", "Main");
			}

            // if (userId == null) return View("Error");
            //UserAccount ua = db.UserAccounts.Find(Session["user"]);
            //if (ua == null) return View("Error");

	        if (currentFolderId == null)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
				return RedirectToAction("Index", "Main");
			}
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			Folder currentFolder = db.Folders.Find(currentFolderId);
	        if (currentFolder == null || currentFolder.OwnerUserAccount != ua)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
				return RedirectToAction("Index", "Main");
			}

            currentFolder.Name = newFolderName;
            currentFolder.DateModified = DateTime.Now;
            //db.Entry(currentFolder).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder.FolderId});
        }

		/// <summary>
		/// Deletes folder, all its subfolders, files and file contents
		/// </summary>
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

	        if (currentFolderId == null)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
				return RedirectToAction("Index", "Main");
			}
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
            Folder currentFolder = db.Folders.Find(currentFolderId);
	        if (currentFolder == null || currentFolder.OwnerUserAccount != ua)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
				return RedirectToAction("Index", "Main");
			}

            Folder parentFolder = currentFolder.ParentFolder;

			List<Folder> folderList = (db.Folders.ToList().Where(f => f.OwnerUserAccount == ua)).ToList();

			List<Folder> folds = new List<Folder>();
	        List<Folder> childFolders = new List<Folder> {currentFolder};
	        while (childFolders.Count > 0)
			{
				Folder tmpFolder = childFolders.First();
				folds.Add(tmpFolder);
				childFolders.AddRange(folderList.FindAll(f => f.ParentFolder == tmpFolder).ToList());
				childFolders.Remove(tmpFolder);
			}

			// remove all files in a folder
            List<File> fileList = (db.Files.ToList().Where(item =>  folds.Contains(item.ParentFolder) && item.OwnerUserAccount == ua)).ToList();
            List<FileContent> fileContentList = (db.FileContents.ToList().Where(item => fileList.Select(x => x.Content).Contains(item))).ToList();

            db.Files.RemoveRange(fileList);
            db.FileContents.RemoveRange(fileContentList);
            db.Folders.RemoveRange(folds); // delete folder
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = parentFolder?.FolderId});
        }

		/// <summary>
		/// Uploads file to the database in selected folder
		/// </summary>
		[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UploadFile(int? currentFolderId, string newFileName, HttpPostedFileBase file)
        {

	        if (string.IsNullOrWhiteSpace(newFileName) ||
	            file == null ||
	            file.ContentLength <= 0)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				return RedirectToAction("Index", "Main");
			}
	        if (file.ContentLength > 20971520) // 20 MB
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.FileTooLarge);
				return RedirectToAction("Index", "Main");
			}

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
	            if (currentFolder == null || currentFolder.OwnerUserAccount != ua)
	            {
					TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
					return RedirectToAction("Index", "Main");
				}
            }

	        FileContent fc = new FileContent();
	        fc.Data = fileData;

	        string fileType = "/";
            string[] possibleTypes = fileName.Split('.');
            if (possibleTypes.Length > 1) fileType = possibleTypes.Last();

			string link;
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
			logger.Info("User "+ ua.Username + " uploaded file with id " + f.FileId + " and size " + f.Size);
			return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId });
        }

		/// <summary>
		/// Downloads selected file
		/// </summary>
		public ActionResult DownloadFile(int? fileId)
        {
            //if (userId == null) return View("Error");
            //UserAccount ua = db.UserAccounts.Find(userId);
            //if (ua == null) return View("Error");

	        if (fileId == null)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFile);
				return RedirectToAction("Index", "Main");
			}
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			File f = db.Files.Find(fileId);
	        if (f == null || f.OwnerUserAccount != ua)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFile);
				return RedirectToAction("Index", "Main");
			}

			db.Entry(f).Reference(c => c.Content).Load();
			//FileContent fc = db.Files.Include(x => x.Content).First(x => x.FileId == fileId).Content; // damn lazy loading is lazy so make it eager
            //if (fc == null || fc.Data.Length <= 0) return View("Error");
	        FileContent fc = f.Content;

            string fileName = f.Name;
            if (f.FileType != "/" && f.Name.Split('.').Last() != f.FileType) fileName += "." + f.FileType;
			logger.Info("User " + ua.Username + " downloaded file with id " + f.FileId + " and size " + f.Size);
			return File(fc.Data, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

		/// <summary>
		/// Deletes selected file
		/// </summary>
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
	            if (currentFolder == null || currentFolder.OwnerUserAccount != ua)
	            {
					TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
					return RedirectToAction("Index", "Main");
				}
            }

	        if (fileId == null)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFile);
				return RedirectToAction("Index", "Main");
			}
            File f = db.Files.Find(fileId);
	        if (f == null || f.OwnerUserAccount != ua)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFile);
				return RedirectToAction("Index", "Main");
			}

			db.Entry(f).Reference(c => c.Content).Load();
	        FileContent fc = f.Content;
			//FileContent fc = db.Files.Include(x => x.Content).FirstOrDefault(x => x.FileId == fileId).Content; // damn lazy loading is lazy so make it eager
			// if (fc == null || fc.Data.Length <= 0) return View("Error");

			db.Files.Remove(f);
            db.FileContents.Remove(fc);
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId });
        }

		/// <summary>
		/// Allows sharing of selected file
		/// </summary>
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
	            if (currentFolder == null || currentFolder.OwnerUserAccount != ua)
	            {
					TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
					return RedirectToAction("Index", "Main");
				}
            }

            if (fileId == null) return View("Error");
            File f = db.Files.Find(fileId);
	        if (f == null || f.OwnerUserAccount != ua)
	        {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFile);
				return RedirectToAction("Index", "Main");
			}
			db.Entry(f).Reference(c => c.Content).Load();

			f.Sharing = 1;
            db.SaveChanges();

            return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId });
        }

		/// <summary>
		/// Stopps sharing of selected file
		/// </summary>
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
				if (currentFolder == null || currentFolder.OwnerUserAccount != ua)
				{
					TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFolder);
					return RedirectToAction("Index", "Main");
				}
			}

			if (fileId == null)
			{
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFile);
				return RedirectToAction("Index", "Main");
			}
			File f = db.Files.Find(fileId);
			if (f == null || f.OwnerUserAccount != ua)
			{
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFile);
				return RedirectToAction("Index", "Main");
			}
			db.Entry(f).Reference(c => c.Content).Load();

			f.Sharing = 0;
			db.SaveChanges();

			return RedirectToAction("OpenFolder", "Main", new {folderId = currentFolder?.FolderId });
		}

		/// <summary>
		/// Downloads file that is being shared
		/// </summary>
		[AllowAnonymous]
		public ActionResult DownloadSharedFile(string url)
	    {
		    if (url == null)
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				return RedirectToAction("Index", "Main");
			}
			File f = db.Files.FirstOrDefault(s => s.Link == url);
		    if (f == null || f.Sharing == 0)
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFile);
				return RedirectToAction("Index", "Main");
			}

			db.Entry(f).Reference(c => c.Content).Load();
			FileContent fc = f.Content;

			string fileName = f.Name;
			if (f.FileType != "/" && f.Name.Split('.').Last() != f.FileType) fileName += "." + f.FileType;

			logger.Info("Shared file " + f.FileId + " with size " + f.Size + " downloaded");
			return File(fc.Data, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
		}

	}
}