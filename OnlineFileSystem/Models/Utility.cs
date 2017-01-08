using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnlineFileSystem.Models
{
	public static class Utility
	{

		public static string GenerateConfirmationLink()
		{
			return null;
		}

		public static string HashPassword(string password, string salt)
		{
			return System.Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(password + salt)));
		}

		public static string GenerateRandomString(int length = 1000)
		{
			byte[] saltByte = new byte[length];
			new RNGCryptoServiceProvider().GetNonZeroBytes(saltByte);
			return System.Convert.ToBase64String(saltByte);
		}

		public static bool SendConfirmationEmail(string email, string confirmationUrl)
		{
			throw new Exception("Configure USERNAME and PASSWORD for EMAIL");
			string username = "USERNAME";
			string password = "PASSWORD";
			
			var smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(username, password)
			};
			using (var message = new MailMessage("OnlineFileServer@TEST.com", email)
			{
				Subject = "Confirmation code for your registered account",
				Body = "We are sending you the confirmation URL: " + confirmationUrl
			})
			{
				smtp.Send(message);
			}
			return true;
		}

		public static bool SendPasswordResetEmail(string email, string newPassword)
		{
			throw new Exception("Configure USERNAME and PASSWORD for EMAIL");
			string username = "USERNAME";
			string password = "PASSWORD";

			var smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(username, password)
			};
			using (var message = new MailMessage("OnlineFileServer@TEST.com", email)
			{
				Subject = "Password recovery",
				Body = "We are sending you the new password: " + newPassword
			})
			{
				smtp.Send(message);
			}
			return true;
		}

		public enum AccountType
		{
			User = 0,
			Unconfirmed = 1,
			Admin = 2
		}

		public static string IntToAccountType(int type)
		{
			switch (type)
			{
				case 0:
					return AccountType.User.ToString();
				case 1:
					return AccountType.Unconfirmed.ToString();
				case 2:
					return AccountType.Admin.ToString();
			}

			return null;
		}
		public static int AccountTypeToInt(AccountType type)
		{
			switch (type)
			{
				case AccountType.User:
					return 0;
				case AccountType.Unconfirmed:
					return 1;
				case AccountType.Admin:
					return 2;
			}

			return -1;
		}
	}
}