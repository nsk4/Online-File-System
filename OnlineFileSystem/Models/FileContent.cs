using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineFileSystem.Models
{
    public class FileContent
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileContentId { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}