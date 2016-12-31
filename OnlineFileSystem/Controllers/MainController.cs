using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Controllers
{
    public class MainController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Main
        public ActionResult Index()
        {
            UserAccount ua = db.UserAccounts.Single(u => u.Username == "TestUser1");
            List<Folder> folderList = (from item in db.Folders.ToList() where item.OwnerUserAccount == ua select item).ToList();
            ViewBag.folderList = folderList;
            List<File> fileList = (from item in db.Files.ToList() where folderList.Contains(item.ParentFolder) select item).ToList();
            ViewBag.fileList = fileList;

            return View();
        }

        public ActionResult GetData(string userId)
        {
            if (userId == null) return View("Error");
            List<UserAccount> uaList = (from account in db.UserAccounts.ToList() where account.Username == userId select account).ToList();
            if (uaList.Count != 1) return View("Error");
            UserAccount ua = uaList.First();
            //UserAccount ua = db.UserAccounts.Single(u => u.Username == userId);

            List<Folder> folderList = (from item in db.Folders.ToList() where item.OwnerUserAccount == ua select item).ToList();
            List<File> fileList = (from item in db.Files.ToList() where item.ParentFolder == null select item).ToList();
            ViewBag.folderList = folderList;
            ViewBag.fileList = fileList;
            ViewBag.userId = userId;

            return View("Index");
        }

        public ActionResult OpenFolder(string userId, int? folderId)
        {
            if (folderId == null) return RedirectToAction("GetData", new {userId = userId});

            if (userId == null) return View("Error");
            List<UserAccount> uaList = (from account in db.UserAccounts.ToList() where account.Username == userId select account).ToList();
            if (uaList.Count != 1) return View("Error");
            UserAccount ua = uaList.First();
            //UserAccount ua = db.UserAccounts.Single(u => u.Username == userId);

            List<Folder> folderList = (from item in db.Folders.ToList() where item.OwnerUserAccount == ua && item.FolderId==folderId select item).ToList();
            Folder currentFolder = folderList.First(f => f.FolderId == folderId);
            List<File> fileList = (from item in db.Files.ToList() where item.ParentFolder==currentFolder select item).ToList();
            folderList.Remove(currentFolder);
            ViewBag.currentFolder = currentFolder;
            ViewBag.folderList = folderList;
            ViewBag.fileList = fileList;
            ViewBag.userId = userId;

            return View("Index");
        }
    }
}