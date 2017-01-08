using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Controllers
{
    public class FrontController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();


		// GET: Front
		public ActionResult Index()
        {
            return View();
        }


	    public ActionResult Login()
	    {
		    return View("Login");
	    }

		public ActionResult Login(string username, string password)
		{
			// TODO: change function signature
			if (username == null || password == null) RedirectToAction("Login");

			UserAccount ua = db.UserAccounts.FirstOrDefault(u => u.Username == username);
			if (ua == null || ua.Password != Utility.HashPassword(password, ua.PasswordSalt)) return View("Login");

			ua.LastLogin = DateTime.Now;
			db.SaveChanges();

			// TODO: logged in

			return View("Login");
		}

		public ActionResult Register()
	    {
		    return View("Register");
	    }

		public ActionResult Register(string username, string password, string passwordRepeat, string email, string emailRepeat)
		{
			// TODO: change function signature
			if (username == null || password == null || passwordRepeat == null || email == null || emailRepeat == null || password != passwordRepeat || email != emailRepeat) RedirectToAction("Register");

			int numOfAccounts = db.UserAccounts.Select(s => s.Username == username).Count();
			if (numOfAccounts != 0) RedirectToAction("Register");
			int numOfEmails = db.UserAccounts.Select(s => s.Email == email).Count();
			if (numOfEmails != 0) RedirectToAction("Register");

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

			return View("Login");
		}

		public ActionResult ForgotPassword()
	    {
			return View("ForgotPassword");
		}

	    public ActionResult ForgotPassword(string email)
	    {
			// TODO: change function signature
			if (email == null) RedirectToAction("ForgotPassword");
			UserAccount ua = db.UserAccounts.FirstOrDefault(u => u.Email == email);
		    if (ua == null) RedirectToAction("ForgotPassword");

		    string password = Utility.GenerateRandomString();
		    if (!Utility.SendPasswordResetEmail(email, password)) return View("Error");
			ua.Password = Utility.HashPassword(password, ua.PasswordSalt);
			ua.DateModified = DateTime.Now;
		    db.SaveChanges();

		    return View("Login");
	    }
    }
}