using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;

namespace OnlineFileSystem.Controllers
{
	/// <summary>
	/// Default home page controller
	/// </summary>
    public class HomeController : Controller
    {
		/// <summary>
		/// Default method that returns front page
		/// </summary>
		public ActionResult Index()
        {
			ViewBag.Error = TempData["Error"];
			return View();
        }
    }
}