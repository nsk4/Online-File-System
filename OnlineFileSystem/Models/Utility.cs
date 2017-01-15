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
			//return true;
			//throw new Exception("Configure USERNAME and PASSWORD for EMAIL");
			string username = "travianus.team@gmail.com";
			string password = "developer";
			try
			{
				using (MailMessage mm = new MailMessage(new MailAddress(username), new MailAddress(email)))
				{
					mm.Subject = "Confirmation code for your registered account";
					mm.Body = "We are sending you the confirmation URL: " + confirmationUrl;

					mm.IsBodyHtml = false;
					SmtpClient smtp = new SmtpClient();
					smtp.Host = "smtp.gmail.com";
					smtp.EnableSsl = true;
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					NetworkCredential NetworkCred = new NetworkCredential(username, password);
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = NetworkCred;
					smtp.Port = 587;
					smtp.Send(mm);
				}
			}
			catch (Exception e)
			{
				return false;
			}

			return true;
		}

		public static bool SendPasswordResetEmail(string email, string newPassword)
		{
			//return true;
			//throw new Exception("Configure USERNAME and PASSWORD for EMAIL");
			string username = "travianus.team@gmail.com";
			string password = "developer";
			try
			{
				using (MailMessage mm = new MailMessage(new MailAddress(username), new MailAddress(email)))
				{
					mm.Subject = "Password recovery";
					mm.Body = "We are sending you the new password: " + newPassword;

					mm.IsBodyHtml = false;
					SmtpClient smtp = new SmtpClient();
					smtp.Host = "smtp.gmail.com";
					smtp.EnableSsl = true;
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					NetworkCredential NetworkCred = new NetworkCredential(username, password);
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = NetworkCred;
					smtp.Port = 587;
					smtp.Send(mm);
				}
			}
			catch (Exception)
			{
				return false;
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

		public enum ErrorType
		{
			InvalidUsernameOrPassword,
			InvalidFolder,
			AccountWithThisUsernameAlreadyExists,
			AccountWithThisEmailAlreadyExists,
			InvalidFormData,
			ErrorInSendingEmail,
			InvalidEmail,
			PasswordDoesNotMatch,
			InvalidFile,
			Unauthorized,
			UnauthorizedUnconfirmed,
			LoginRequired,
			FileTooLarge,


			Error400,
			Error403,
			Error404,
			Error40413,
			Error500
		}

		public static string GetErrorMessage(ErrorType et)
		{
			string message = "";
			switch (et)
			{
				case ErrorType.InvalidUsernameOrPassword:
					message = "Username and/or password is invalid.";
					break;
				case ErrorType.InvalidFolder:
					message = "Folder is not valid.";
					break;
				case ErrorType.AccountWithThisUsernameAlreadyExists:
					message = "Account with this username already exists.";
					break;
				case ErrorType.AccountWithThisEmailAlreadyExists:
					message = "Account with this email already exists.";
					break;
				case ErrorType.InvalidFormData:
					message = "Form data is invalid.";
					break;
				case ErrorType.ErrorInSendingEmail:
					message = "Error in sending email.";
					break;
				case ErrorType.InvalidEmail:
					message = "There is no user with this email.";
					break;
				case ErrorType.PasswordDoesNotMatch:
					message = "Password does not match.";
					break;
				case ErrorType.InvalidFile:
					message = "File is not valid.";
					break;
				case ErrorType.Unauthorized:
					message = "You are not authorised to see this page.";
					break;
				case ErrorType.UnauthorizedUnconfirmed:
					message = "Unauthorized, confirm your account first.";
					break;
				case ErrorType.LoginRequired:
					message = "Unauthorized, log in first.";
					break;
				case ErrorType.FileTooLarge:
					message = "File is too large.";
					break;
				case ErrorType.Error400:
					message = "Error 400: Bad request.";
					break;
				case ErrorType.Error403:
					message = "Error 403: You are fobidden to access this resource.";
					break;
				case ErrorType.Error404:
					message = "Error 404: Page not found.";
					break;
				case ErrorType.Error500:
					message = "Error 500: There was an internal server error.";
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(et), et, null);
			}

			return message;
		}
	}
}