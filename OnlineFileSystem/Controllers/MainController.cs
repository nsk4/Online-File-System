using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            ViewBag.TmpList = (from item in db.Folders.ToList() where item.OwnerUserAccount == ua select item).ToList();
            //ViewBag.TmpList = db.Folders.Where(f => f.OwnerUserAccount == ua).ToList();
            ViewBag.Title = "qwe1";
            ViewBag.Message = "ewq";

            
            return View();
        }
    }
}