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
			ViewBag.UserAccountId = ua.UserAccountId;
	        ViewBag.Username = ua.Username;
	        ViewBag.Email = ua.Email;
	        ViewBag.DateModified = ua.DateModified;
	        ViewBag.DateCreated = ua.DateCreated;
	        ViewBag.LastLogin = ua.LastLogin;
	        ViewBag.Role = ua.Role;

			return View();
        }

	    public ActionResult OpenAccountOptions(int? userId)
	    {
		    if (userId == null) return View("Error");

			UserAccount ua = db.UserAccounts.Single(u => u.UserAccountId == userId);
		    if (ua == null) return View("Error");

			ViewBag.UserAccountId = ua.UserAccountId;
			ViewBag.Username = ua.Username;
			ViewBag.Email = ua.Email;
			ViewBag.DateModified = ua.DateModified;
			ViewBag.DateCreated = ua.DateCreated;
			ViewBag.LastLogin = ua.LastLogin;
			ViewBag.Role = ua.Role;

			return View("Index");
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public ActionResult ChangePassword(int? userId, string oldPassword, string newPassword, string newPasswordRepeat)
	    {
			if (userId == null || newPassword != newPasswordRepeat || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(newPasswordRepeat)) return View("Error");

			UserAccount ua = db.UserAccounts.Find(userId);
			if(ua == null) return View("Error");

			string oldPasswordHashed = Utility.HashPassword(oldPassword, ua.PasswordSalt);
			if (ua.Password != oldPasswordHashed) return View("Error");

		    ua.PasswordSalt = Utility.GenerateRandomString();
		    ua.Password = Utility.HashPassword(newPassword, ua.PasswordSalt);
		    ua.DateModified = DateTime.Now;

			db.SaveChanges();

			return RedirectToAction("OpenAccountOptions", "AccountOptions", new { userId = userId });
			
		}

	    public ActionResult SendActivationEmail(int? userId)
	    {
		    if (userId == null) return View("Error");
			UserAccount ua = db.UserAccounts.Find(userId);
			if (ua == null || ua.Role != (int)Utility.AccountType.Unconfirmed) return View("Error");

			//string link = HtmlHelper.GenerateLink(this.ControllerContext.RequestContext, System.Web.Routing.RouteTable.Routes, "Confirm email", "Root", "ConfirmEmail", "AccountOptions", { userId = ua.UserAccountId, confirmationLink = ua.Confirmationlink }, null);
			
			UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
			string url = u.Action("ConfirmEmail", "AccountOptions", new { userId = ua.UserAccountId, confirmationLink = ua.Confirmationlink });
		    url = Request.Url.GetLeftPart(UriPartial.Authority) + url;
			if (!Utility.SendConfirmationEmail(ua.Email, url)) return View("Error");

			return RedirectToAction("OpenAccountOptions", "AccountOptions", new { userId = userId });
		}

	    public ActionResult ConfirmEmail(int? userId, string confirmationLink)
	    {
			if (userId == null || string.IsNullOrWhiteSpace(confirmationLink)) return View("Error");
			UserAccount ua = db.UserAccounts.Find(userId);
			if (ua == null || ua.Role != (int)Utility.AccountType.Unconfirmed || ua.Confirmationlink != confirmationLink) return View("Error");

		    ua.Role = (int)Utility.AccountType.User;
			ua.DateModified = DateTime.Now;
			db.SaveChanges();
			
			return RedirectToAction("OpenAccountOptions", "AccountOptions", new { userId = userId });
		}

	}
}