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
		readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private ApplicationDbContext db = new ApplicationDbContext();

		/// <summary>
		/// Verifies the login data and is they are correct the session is started
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(string username, string password)
		{
			// TODO: change function signature
			if (username.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace())
			{
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				return RedirectToAction("Index", "Home");
			}

			UserAccount ua = db.UserAccounts.FirstOrDefault(u => u.Username == username);
			if (ua == null || ua.Password != Utility.HashPassword(password, ua.PasswordSalt))
			{
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidUsernameOrPassword);
				logger.Info("Invalid username/password for username " + username);
				return RedirectToAction("Index", "Home");
			}

			ua.LastLogin = DateTime.Now;
			db.SaveChanges();
			logger.Info("User logged in " + ua.Username);
			// TODO: logged in
			Session["user"] = ua;
			
			return RedirectToAction("Index", "Main");
		}

		/// <summary>
		/// Registers the new user to the system
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(string username, string password, string passwordRepeat, string email, string emailRepeat)
		{
			if (username.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace() || passwordRepeat.IsNullOrWhiteSpace() ||
			    email.IsNullOrWhiteSpace() || emailRepeat.IsNullOrWhiteSpace() || password != passwordRepeat ||
			    email != emailRepeat)
			{
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				return RedirectToAction("Index", "Home");
			}
				
			int numOfAccounts = db.UserAccounts.Count(s => s.Username == username);
			if (numOfAccounts != 0)
			{
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.AccountWithThisUsernameAlreadyExists);
				logger.Info("Account already exists for username " + username);
				return RedirectToAction("Index", "Home");
			}
			int numOfEmails = db.UserAccounts.Count(s => s.Email == email);
			if (numOfEmails != 0)
			{
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.AccountWithThisEmailAlreadyExists);
				logger.Info("Account already exists for email " + email);
				return RedirectToAction("Index", "Home");
			}

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

		/// <summary>
		/// Sends a new temporary password to the user
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ForgotPassword(string email)
	    {
		    if (email.IsNullOrWhiteSpace())
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				return RedirectToAction("Index", "Home");
		    }
			UserAccount ua = db.UserAccounts.FirstOrDefault(u => u.Email == email);
		    if (ua == null)
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidEmail);
				return RedirectToAction("Index", "Home");
		    }

			string password = Utility.GenerateRandomString();
		    if (!Utility.SendPasswordResetEmail(email, password))
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.ErrorInSendingEmail);
				logger.Warn("Error in sending password reset email to user " + ua.Username);
				return RedirectToAction("Index", "Home");
			}
			ua.Password = Utility.HashPassword(password, ua.PasswordSalt);
			ua.DateModified = DateTime.Now;
		    db.SaveChanges();
			
		    return RedirectToAction("Index", "Home");
		}

		/// <summary>
		/// Clears the session thus logging user out
		/// </summary>
		public ActionResult Logout()
	    {
			Session.RemoveAll();
			return RedirectToAction("Index", "Home");
	    }
    }
}