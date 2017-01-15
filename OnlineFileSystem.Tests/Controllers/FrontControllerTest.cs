using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineFileSystem.Controllers;
using OnlineFileSystem.Models;
using Moq;

namespace OnlineFileSystem.Tests.Controllers
{
	[TestClass]
	public class FrontControllerTest
	{
		[TestMethod]
		public void Login()
		{
			// http://www.c-sharpcorner.com/uploadfile/raj1979/unit-testing-in-mvc-4-using-entity-framework/
			var mock = new Mock<ControllerContext>();
			var mockSession = new Mock<HttpSessionStateBase>();
			mock.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
			var controller = new FrontController();
			controller.ControllerContext = mock.Object;
			// Act
			var result = controller.Login("nsk", "QWEqwe123");
			Assert.IsNotNull(controller.HttpContext.Session["user"]);
		}

		[TestMethod]
		public void Register()
		{

		}

		[TestMethod]
		public void ForgotPassword()
		{

		}

		[TestMethod]
		public void Logout()
		{
			UserAccount ua = new UserAccount();
			ua.Username = "qwe";
			var mock = new Mock<ControllerContext>();
			var mockSession = new Mock<HttpSessionStateBase>();
			mock.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
			var controller = new FrontController();
			controller.ControllerContext = mock.Object;
			// Act
			var result = controller.Logout();
			Assert.IsNull(controller.HttpContext.Session["user"]);
		}
	}
}
