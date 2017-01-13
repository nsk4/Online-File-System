using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineFileSystem.Models
{
	public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
	{
		public new void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
				|| filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
			{
				// Don't check for authorization as AllowAnonymous filter is applied to the action or controller
				return;
			}

			UserAccount ua = (UserAccount)HttpContext.Current.Session["user"];
			if (ua == null)
			{
				filterContext.Result = new HttpUnauthorizedResult();
				filterContext.Controller.TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.LoginRequired);
				return;
			}
			if (this.UserProfilesRequired.Length == 0) return;

			bool authorized = false;
			foreach (var role in this.UserProfilesRequired)
			{
				if (ua.Role == Utility.AccountTypeToInt(role))
				{
					authorized = true;
					break;
				}
			}
			if (!authorized)
			{
				if (ua.Role == Utility.AccountTypeToInt(Utility.AccountType.Unconfirmed))
				{
					filterContext.Controller.TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.UnauthorizedUnconfirmed);
				}
					
				else
				{
					filterContext.Controller.TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.Unauthorized);
				}
				filterContext.Result = new HttpUnauthorizedResult();

				return;
			}

			// Check for authorization
			
		}

		private Utility.AccountType[] UserProfilesRequired { get; set; }
		public AuthorizationFilter(params Utility.AccountType[] userProfilesRequired)
		{
			if (userProfilesRequired.Any(p => p.GetType().BaseType != typeof(Enum)))
				throw new ArgumentException("userProfilesRequired");
			this.UserProfilesRequired = userProfilesRequired.ToArray();
			//this.UserProfilesRequired = userProfilesRequired.Select(p => Enum.GetName(p.GetType(), p)).ToArray();
		}
	}
}