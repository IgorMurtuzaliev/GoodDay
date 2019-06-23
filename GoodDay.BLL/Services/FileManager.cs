using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File = GoodDay.Models.Entities.File;

namespace GoodDay.BLL.Services
{
    public class FileManager : IFileManager
    {
        public IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment appEnvironment;
        public FileManager(IUnitOfWork _unitOfWork, IHostingEnvironment _appEnvironment)
        {
            unitOfWork = _unitOfWork;
            appEnvironment = _appEnvironment;
        }
        public async Task<File> EditImage(User user, IFormFile file)
        {
            string path = "\\Avatar\\" + user.UserName + "\\" + file.FileName;
            string directory = Path.Combine(appEnvironment.WebRootPath + "\\Avatar\\" + user.UserName + "\\");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (var fileStream = new FileStream(directory + file.FileName, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            File newfile = new File { Name = file.FileName, Path = path };
            return newfile;
        }
        public async Task<ICollection<File>> UploadMessagesFiles(int dialogId, int messageId, IFormFileCollection files)
        {
            string directory = Path.Combine(appEnvironment.WebRootPath + "\\Dialogs\\" + dialogId + "\\");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var fileCollection = new List<File>();
            foreach (var file in files)
            {
                string path = "\\Dialogs\\" + dialogId.ToString() + "\\" + file.FileName;
                using (var fileStream = new FileStream(directory + file.FileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                File newfile = new File { Name = file.FileName, Path = path, MessageId = messageId};
                await unitOfWork.Files.Add(newfile);
                fileCollection.Add(newfile);
            }
            return fileCollection;
        }
    }
}
