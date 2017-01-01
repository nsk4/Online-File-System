using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineFileSystem.Models.ViewModels
{
    [Obsolete]
    public class CreateFolderViewModel
    {
        [Display(Name = "Folder name")]
        public string FolderName { get; set; }

        [Required]
        public int ParentFolderId;
    }
}