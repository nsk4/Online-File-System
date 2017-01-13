using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Controllers
{
    public class FrontController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(string username, string password)
		{
			// TODO: change function signature
			if (username.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace()) return RedirectToAction("Index", "Home");

			UserAccount ua = db.UserAccounts.FirstOrDefault(u => u.Username == username);
			if (ua == null || ua.Password != Utility.HashPassword(password, ua.PasswordSalt)) return RedirectToAction("Index", "Home");

			ua.LastLogin = DateTime.Now;
			db.SaveChanges();

			// TODO: logged in
			Session["user"] = ua;
			
			return RedirectToAction("OpenFolder", "Main");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(string username, string password, string passwordRepeat, string email, string emailRepeat)
		{
			// TODO: change function signature
			if (username.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace() || passwordRepeat.IsNullOrWhiteSpace() || email.IsNullOrWhiteSpace() || emailRepeat.IsNullOrWhiteSpace() || password != passwordRepeat || email != emailRepeat) return RedirectToAction("Index", "Home");
			int numOfAccounts = db.UserAccounts.Count(s => s.Username == username);
			if (numOfAccounts != 0) return RedirectToAction("Index", "Home");
			int numOfEmails = db.UserAccounts.Count(s => s.Email == email);
			if (numOfEmails != 0) return RedirectToAction("Index", "Home");

			string link = null;
			do
			{
				link = (username + DateTime.Now.ToString("U") + (new Random()).Next(0, 1000).ToString()).GetHashCode().ToString();
			} while (db.UserAccounts.Any(x => x.Confirmationlink == link));

			UserAccount newUser = new UserAccount();
			newUser.Username = username;
			newUser.PasswordSalt = Utility.GenerateRandomString();
			newUser.Password = Utility.HashPassword(password, newUser.PasswordSalt);
			newUser.Email = email;
			newUser.Role = Utility.AccountTypeToInt(Utility.AccountType.Unconfirmed);
			newUser.Confirmationlink = link;
			newUser.DateCreated = DateTime.Now;
			newUser.DateModified = DateTime.Now;
			newUser.LastLogin = DateTime.Now;

			db.UserAccounts.Add(newUser);
			db.SaveChanges();

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ForgotPassword(string email)
	    {
			// TODO: change function signature
			if (email.IsNullOrWhiteSpace()) return RedirectToAction("Index", "Home");
			UserAccount ua = db.UserAccounts.FirstOrDefault(u => u.Email == email);
		    if (ua == null) return RedirectToAction("Index", "Home");

			string password = Utility.GenerateRandomString();
		    if (!Utility.SendPasswordResetEmail(email, password)) return View("Error");
			ua.Password = Utility.HashPassword(password, ua.PasswordSalt);
			ua.DateModified = DateTime.Now;
		    db.SaveChanges();
			
		    return RedirectToAction("Index", "Home");
		}

	    public ActionResult Logout()
	    {
			Session.RemoveAll();
			return RedirectToAction("Index", "Home");
	    }
    }
}