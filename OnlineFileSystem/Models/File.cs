using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineFileSystem.Models
{
    public class File
    {
        public File()
        {

        }


        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Sharing { get; set; }

        public string Link { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }

        [Required]
        public FileContent Content { get; set; }

        [Required]
        public Folder ParentFolder { get; set; }
    }
}