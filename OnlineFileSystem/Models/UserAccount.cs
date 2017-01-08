using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OnlineFileSystem.Models
{
    public class UserAccount
    {
        public UserAccount()
        {

        }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAccountId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordSalt { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastLogin { get; set; }


	    public static string HashPassword(string password, string salt)
	    {
			return System.Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(password + salt)));
	    }

	    public static string GenerateSalt(int length = 1000)
	    {
			byte[] saltByte = new byte[length];
			new RNGCryptoServiceProvider().GetNonZeroBytes(saltByte);
		    return System.Convert.ToBase64String(saltByte);
	    }
    }
}