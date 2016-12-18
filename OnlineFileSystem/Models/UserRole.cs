using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int UserRoleId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Role { get; set; }
    }
}