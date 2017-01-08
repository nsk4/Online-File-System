using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Controllers
{
    public class AccountOptionsController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: AccountOptions
		public ActionResult Index()
        {

			UserAccount ua = db.UserAccounts.Single(u => u.Username == "TestUser2");
			db.Entry(ua).Reference(c => c.Role).Load();
			ViewBag.UserAccountId = ua.UserAccountId;
	        ViewBag.Username = ua.Username;
	        ViewBag.Email = ua.Email;
	        ViewBag.DateModified = ua.DateModified;
	        ViewBag.DateCreated = ua.DateCreated;
	        ViewBag.LastLogin = ua.LastLogin;
	        ViewBag.Role = ua.Role.Role;

			return View();
        }

	    public ActionResult OpenAccountOptions(int? userId)
	    {
		    if (userId == null) return View("Error");

			UserAccount ua = db.UserAccounts.Single(u => u.UserAccountId == userId);
		    if (ua == null) return View("Error");

			db.Entry(ua).Reference(c => c.Role).Load();
			ViewBag.UserAccountId = ua.UserAccountId;
			ViewBag.Username = ua.Username;
			ViewBag.Email = ua.Email;
			ViewBag.DateModified = ua.DateModified;
			ViewBag.DateCreated = ua.DateCreated;
			ViewBag.LastLogin = ua.LastLogin;
			ViewBag.Role = ua.Role.Role;

			return View("Index");
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public ActionResult ChangePassword(int? userId, string oldPassword, string newPassword, string newPasswordRepeat)
	    {
			if (userId == null || newPassword != newPasswordRepeat || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(newPasswordRepeat)) return View("Error");

			UserAccount ua = db.UserAccounts.Find(userId);
			if(ua == null) return View("Error");
			db.Entry(ua).Reference(c => c.Role).Load();

			string oldPasswordHashed = UserAccount.HashPassword(oldPassword, ua.PasswordSalt);
			if (ua.Password != oldPasswordHashed) return View("Error");



		    ua.PasswordSalt = UserAccount.GenerateSalt();
		    ua.Password = UserAccount.HashPassword(newPassword, ua.PasswordSalt);

			db.SaveChanges();

			return RedirectToAction("OpenAccountOptions", "AccountOptions", new { userId = userId });
			
		}

	    public ActionResult ResendActivationEmail(int? userId)
	    {
		    if (userId == null) return View("Error");
			UserAccount ua = db.UserAccounts.Find(userId);
			if (ua == null) return View("Error");

		    string email = ua.Email;

			// TODO: sending of email

			return RedirectToAction("OpenAccountOptions", "AccountOptions", new { userId = userId });
		}

	}
}