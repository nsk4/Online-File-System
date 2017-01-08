using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineFileSystem.Models
{
    public class UserRole
    {
        public UserRole()
        {

        }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Role { get; set; }
    }

	public enum AccountType
	{
		User = 0,
		Unconfirmed = 1,
		Admin = 2
	}
}