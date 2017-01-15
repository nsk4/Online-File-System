using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Errors
{
	
	public class ErrorController : Controller
    {
		public ActionResult Index()
        {

            return RedirectToAction("Index", "Home");
        }

		public ActionResult Error400()
		{
			TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.Error400);
			return RedirectToAction("Index", "Home");
		}
		public ActionResult Error403()
		{
			TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.Error403);
			return RedirectToAction("Index", "Home");
		}
		public ActionResult Error404()
		{
			TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.Error404);
			return RedirectToAction("Index", "Home");
		}
		public ActionResult Error40413()
		{
			TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.Error40413);
			return RedirectToAction("Index", "Home");
		}
		public ActionResult Error500()
		{
			TempData["Error"] = Utility.GetErrorMessage(Utility.ErrorType.Error500);
			return RedirectToAction("Index", "Home");
		}

	}
}