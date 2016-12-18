using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace OnlineFileSystem.Models
{
    public class Folder
    {
        public Folder()
        {

        }

        [Required]
        public int FolderId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }

        
        public Folder ParentFolder { get; set; }

        [Required]
        public UserAccount OwnerUserAccount { get; set; }

        //public virtual ICollection<File> Files { get; set; }
        //public virtual ICollection<Folder> Folders { get; set; }
    }
}