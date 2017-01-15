using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Tests.Models
{
	[TestClass]
	public class UtilityTest
	{
		[TestMethod]
		public void HashPasswordPositiveTest()
		{
			string pass = "qwe";
			string salt = "qwe";
			string expectedPass = System.Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(pass + salt)));
			string calculatedPass = Utility.HashPassword(pass, salt);
			Assert.AreEqual(expectedPass, calculatedPass);
		}

		[TestMethod]
		public void GenerateRandomStringNegativeTest()
		{
			try
			{
				Utility.GenerateRandomString(-1);
				Assert.Fail("Should throw and error.");
			}
			catch (Exception e)
			{
				Assert.IsNotNull(e);
				return;
			}
		}

		[TestMethod]
		public void SendConfirmationEmailPositiveTest()
		{
			bool pass = Utility.SendConfirmationEmail("travianus.team@gmail.com", "test mail");
			Assert.IsTrue(pass);
		}

		[TestMethod]
		public void SendConfirmationEmailNegativeTest()
		{
			bool pass = Utility.SendConfirmationEmail("kekec", "test mail");
			Assert.IsFalse(pass);
		}

		[TestMethod]
		public void SendPasswordResetEmailPositiveTest()
		{
			bool pass = Utility.SendPasswordResetEmail("travianus.team@gmail.com", "test pass");
			Assert.IsTrue(pass);
		}

		[TestMethod]
		public void SendPasswordResetEmailNegativeTest()
		{
			bool pass = Utility.SendPasswordResetEmail("kekec", "test pass");
			Assert.IsFalse(pass);
		}

		[TestMethod]
		public void IntToAccountTypePositiveTest()
		{
			string accountType = Utility.IntToAccountType(0);
			Assert.AreEqual(accountType, Utility.AccountType.User.ToString());
		}

		[TestMethod]
		public void IntToAccountTypeNegativeTest1()
		{
			string accountType = Utility.IntToAccountType(5);
			Assert.IsNull(accountType);
		}

		[TestMethod]
		public void IntToAccountTypeNegativeTest2()
		{
			string accountType = Utility.IntToAccountType(-1);
			Assert.IsNull(accountType);
		}

		[TestMethod]
		public void AccountTypeToIntPositiveTest()
		{
			int accountType = Utility.AccountTypeToInt(Utility.AccountType.User);
			Assert.AreEqual(accountType, 0);
		}

	}
}
