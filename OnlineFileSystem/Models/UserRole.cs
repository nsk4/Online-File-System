using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineFileSystem.Models
{
    public class UserRole
    {
        [Required]
        public int UserRoleId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}