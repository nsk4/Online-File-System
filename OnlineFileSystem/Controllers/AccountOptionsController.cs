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
		readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private ApplicationDbContext db = new ApplicationDbContext();

		/// <summary>
		/// Returns the default view for account options
		/// </summary>
		[AuthorizationFilter]
		public ActionResult Index()
        {
			ViewBag.Error = TempData["Error"];
			return View("Index");
        }

		/// <summary>
		/// Changes account password
		/// </summary>
		[ValidateAntiForgeryToken]
		[HttpPost]
		[AuthorizationFilter]
		public ActionResult ChangePassword(string oldPassword, string newPassword, string newPasswordRepeat)
	    {
		    if (newPassword != newPasswordRepeat || string.IsNullOrWhiteSpace(newPassword) ||
		        string.IsNullOrWhiteSpace(newPasswordRepeat))
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
			    return RedirectToAction("Index", "AccountOptions");
		    }

			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			string oldPasswordHashed = Utility.HashPassword(oldPassword, ua.PasswordSalt);
		    if (ua.Password != oldPasswordHashed)
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.PasswordDoesNotMatch);
				return RedirectToAction("Index", "AccountOptions");
			}

		    ua.PasswordSalt = Utility.GenerateRandomString();
		    ua.Password = Utility.HashPassword(newPassword, ua.PasswordSalt);
		    ua.DateModified = DateTime.Now;
			db.SaveChanges();

			return RedirectToAction("Index", "AccountOptions");
			
		}

		/// <summary>
		/// Sends email to the user to confirm their account
		/// </summary>
		[AuthorizationFilter(Utility.AccountType.Unconfirmed)]
		public ActionResult SendActivationEmail()
	    {
			UserAccount ua = db.UserAccounts.Find(((UserAccount)Session["user"]).UserAccountId);
			UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
			string url = u.Action("ConfirmEmail", "AccountOptions", new { userId = ua.UserAccountId, confirmationLink = ua.Confirmationlink });
		    url = Request.Url.GetLeftPart(UriPartial.Authority) + url;
		    if (!Utility.SendConfirmationEmail(ua.Email, url))
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.ErrorInSendingEmail);
				logger.Warn("Error in sending email confirmation to user " + ua.Username);
				return RedirectToAction("Index", "AccountOptions");
		    }

			return RedirectToAction("Index", "AccountOptions");
		}

		/// <summary>
		/// Is called when unconfirmed user wants to confirm their email
		/// </summary>
		[AllowAnonymous]
		public ActionResult ConfirmEmail(string confirmationLink)
	    {
		    if (string.IsNullOrWhiteSpace(confirmationLink))
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				if(Session["user"]== null) return RedirectToAction("Index", "Home");
				else return RedirectToAction("Index", "AccountOptions");
			}

			UserAccount ua = db.UserAccounts.FirstOrDefault(u => u.Confirmationlink == confirmationLink);
		    if (ua == null || ua.Role != (int) Utility.AccountType.Unconfirmed || ua.Confirmationlink != confirmationLink)
		    {
				TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.InvalidFormData);
				logger.Warn("Error confirming email for user " + ua.Username);
				if (Session["user"] == null) return RedirectToAction("Index", "Home");
				else return RedirectToAction("Index", "AccountOptions");
			}

		    ua.Role = (int)Utility.AccountType.User;
			ua.DateModified = DateTime.Now;
			db.SaveChanges();
			
			return RedirectToAction("Index", "AccountOptions");
		}

	}
}