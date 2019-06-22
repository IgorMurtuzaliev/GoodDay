using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using File = GoodDay.Models.Entities.File;

namespace GoodDay.BLL.ViewModels
{
    public class FileViewModel
    {
        public string FilesPath { get; set; }
        public string Extension { get; set; }

        public FileViewModel(File file)
        {
            Extension = Path.GetExtension(file.Path);
            FilesPath = file.Path;
        }
    }
}
