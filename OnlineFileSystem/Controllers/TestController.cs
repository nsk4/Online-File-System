using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Controllers
{
    public class TempTestClass
    {
        public int Test1 { get; set; }
        public string Test2 { get; set; }

        public override string ToString()
        {
            return this.Test1.ToString() + "|" + this.Test2;
        }
    }

    public class TestController : Controller
    {
        public string GetSthClass()
        {
            TempTestClass ttc = new TempTestClass();
            ttc.Test1 = 1;
            ttc.Test2 = "Wut zis work1";
            return ttc.ToString();
        }

        public string GetString()
        {
            return "One empire to rule them all!!";
        }

        public ActionResult GetView()
        {
            

            UserRole urc = new UserRole();
            urc.UserRoleId = 88;
            urc.Role = "Kebab";

            //ViewData["Role"] = urc;
            //ViewBag.Role = urc;
            return View("MyView", urc);


        }
    }
}