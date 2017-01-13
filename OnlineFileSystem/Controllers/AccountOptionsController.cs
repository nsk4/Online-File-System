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
	[AuthorizationFilter]
	public class AccountOptionsController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();

		[AuthorizationFilter]
		// GET: AccountOptions
		public ActionResult Index()
        {
			/*
			UserAccount ua = db.UserAccounts.Single(u => u.Username == "TestUser2");
			ViewBag.UserAccountId = ua.UserAccountId;
	        ViewBag.Username = ua.Username;
	        ViewBag.Email = ua.Email;
	        ViewBag.DateModified = ua.DateModified;
	        ViewBag.DateCreated = ua.DateCreated;
	        ViewBag.LastLogin = ua.LastLogin;
	        ViewBag.Role = ua.Role;
			*/
			return View("Index");
        }

		[Obsolete]
		[AuthorizationFilter]
		public ActionResult OpenAccountOptions()
	    {
		    //if (userId == null) return View("Error");

			//UserAccount ua = db.UserAccounts.Single(u => u.UserAccountId == userId);
			//UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			//if (ua == null) return View("Error");
			/*
			ViewBag.UserAccountId = ua.UserAccountId;
			ViewBag.Username = ua.Username;
			ViewBag.Email = ua.Email;
			ViewBag.DateModified = ua.DateModified;
			ViewBag.DateCreated = ua.DateCreated;
			ViewBag.LastLogin = ua.LastLogin;
			ViewBag.Role = ua.Role;
			*/
			return View("Index");
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		[AuthorizationFilter]
		public ActionResult ChangePassword(string oldPassword, string newPassword, string newPasswordRepeat)
	    {
			if (newPassword != newPasswordRepeat || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(newPasswordRepeat)) return View("Error");

			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			//if (ua == null) return View("Error");

			string oldPasswordHashed = Utility.HashPassword(oldPassword, ua.PasswordSalt);
			if (ua.Password != oldPasswordHashed) return View("Error");

		    ua.PasswordSalt = Utility.GenerateRandomString();
		    ua.Password = Utility.HashPassword(newPassword, ua.PasswordSalt);
		    ua.DateModified = DateTime.Now;

			db.SaveChanges();

			return RedirectToAction("Index", "AccountOptions");
			
		}

		[AuthorizationFilter]
		public ActionResult SendActivationEmail()
	    {
			//if (userId == null) return View("Error");
			//UserAccount ua = db.UserAccounts.Find(userId);
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			if (ua.Role != (int)Utility.AccountType.Unconfirmed) return View("Error");

			//string link = HtmlHelper.GenerateLink(this.ControllerContext.RequestContext, System.Web.Routing.RouteTable.Routes, "Confirm email", "Root", "ConfirmEmail", "AccountOptions", { userId = ua.UserAccountId, confirmationLink = ua.Confirmationlink }, null);

			
			UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
			string url = u.Action("ConfirmEmail", "AccountOptions", new { userId = ua.UserAccountId, confirmationLink = ua.Confirmationlink });
		    url = Request.Url.GetLeftPart(UriPartial.Authority) + url;
			if (!Utility.SendConfirmationEmail(ua.Email, url)) return View("Error");

			return RedirectToAction("Index", "AccountOptions");
		}

		[AllowAnonymous]
		public ActionResult ConfirmEmail(string confirmationLink)
	    {
			if (string.IsNullOrWhiteSpace(confirmationLink)) return View("Error");
			//UserAccount ua = db.UserAccounts.Find(userId);
			UserAccount ua = db.UserAccounts.FirstOrDefault(u => u.Confirmationlink == confirmationLink);
			if (ua == null || ua.Role != (int)Utility.AccountType.Unconfirmed || ua.Confirmationlink != confirmationLink) return View("Error");

		    ua.Role = (int)Utility.AccountType.User;
			ua.DateModified = DateTime.Now;
			db.SaveChanges();
			
			return RedirectToAction("Index", "AccountOptions");
		}

	}
}