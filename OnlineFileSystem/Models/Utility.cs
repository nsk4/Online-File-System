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
	/// <summary>
	/// Helper class that includes shared functions from different classes
	/// </summary>
	public static class Utility
	{
		static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Hashes password with salt and converts it to Base64
		/// </summary>
		/// <param name="password"></param>
		/// <param name="salt"></param>
		/// <returns></returns>
		public static string HashPassword(string password, string salt)
		{
			return System.Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(password + salt)));
		}

		/// <summary>
		/// Generates random string to be used as salt
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string GenerateRandomString(int length = 1000)
		{
			byte[] saltByte = new byte[length];
			new RNGCryptoServiceProvider().GetNonZeroBytes(saltByte);
			return System.Convert.ToBase64String(saltByte);
		}

		/// <summary>
		/// Sends an email to the user with link to confirm their account
		/// </summary>
		/// <param name="email"></param>
		/// <param name="confirmationUrl"></param>
		/// <returns></returns>
		public static bool SendConfirmationEmail(string email, string confirmationUrl)
		{
			//return true;
			//throw new Exception("Configure USERNAME and PASSWORD for EMAIL");
			string username = "nejc328@hotmail.com";
			string password = "QWEqwe123";
			try
			{
				SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
				var mail = new MailMessage();
				mail.From = new MailAddress(username);
				mail.To.Add(email);
				mail.Subject = "Confirmation code for your registered account";
				mail.IsBodyHtml = true;
				string htmlBody;
				htmlBody = "We are sending you the confirmation URL: " + confirmationUrl;
				mail.Body = htmlBody;
				SmtpServer.Port = 587;
				SmtpServer.UseDefaultCredentials = false;
				SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
				SmtpServer.EnableSsl = true;
				SmtpServer.Send(mail);

				/*
				using (MailMessage mm = new MailMessage(new MailAddress(username), new MailAddress(email)))
				{
					mm.Subject = "Confirmation code for your registered account";
					mm.Body = "We are sending you the confirmation URL: " + confirmationUrl;

					mm.IsBodyHtml = false;
					SmtpClient smtp = new SmtpClient();
					//smtp.Host = "smtp.gmail.com";
					smtp.EnableSsl = true;
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					NetworkCredential NetworkCred = new NetworkCredential(username, password);
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = NetworkCred;
					smtp.Port = 587;
					smtp.Send(mm);
				}
				*/
			}
			catch (Exception e)
			{
				logger.Warn("Error in sending email " + e.Message);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Sends an email to the user with new temporary password
		/// </summary>
		/// <param name="email"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		public static bool SendPasswordResetEmail(string email, string newPassword)
		{
			//return true;
			//throw new Exception("Configure USERNAME and PASSWORD for EMAIL");
			string username = "nejc328@hotmail.com";
			string password = "QWEqwe123";
			try
			{
				SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
				var mail = new MailMessage();
				mail.From = new MailAddress(username);
				mail.To.Add(email);
				mail.Subject = "Password recovery";
				mail.IsBodyHtml = true;
				string htmlBody;
				htmlBody = "We are sending you the new password: " + newPassword;
				mail.Body = htmlBody;
				SmtpServer.Port = 587;
				SmtpServer.UseDefaultCredentials = false;
				SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
				SmtpServer.EnableSsl = true;
				SmtpServer.Send(mail);

				/*
				using (MailMessage mm = new MailMessage(new MailAddress(username), new MailAddress(email)))
				{
					mm.Subject = "Password recovery";
					mm.Body = "We are sending you the new password: " + newPassword;

					mm.IsBodyHtml = false;
					SmtpClient smtp = new SmtpClient();
					//smtp.Host = "smtp.gmail.com";
					smtp.EnableSsl = true;
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					NetworkCredential NetworkCred = new NetworkCredential(username, password);
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = NetworkCred;
					smtp.Port = 587;
					smtp.Send(mm);
				}
				*/
			}
			catch (Exception e)
			{
				logger.Warn("Error in sending email " + e.Message);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Account type based on values from database
		/// </summary>
		public enum AccountType
		{
			User = 0,
			Unconfirmed = 1,
			Admin = 2
		}

		/// <summary>
		/// Converts account type from integer to account type
		/// </summary>
		/// <param name="type">Integer value of AccountType</param>
		/// <returns></returns>
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

		/// <summary>
		/// Converts account type from AccountType to int
		/// </summary>
		/// <param name="type">AccountType to convert</param>
		/// <returns></returns>
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

		/// <summary>
		/// List of possible custom errors
		/// </summary>
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

		/// <summary>
		/// Generates error message for given error
		/// </summary>
		/// <param name="et">Error type of error</param>
		/// <returns>Error message string</returns>
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