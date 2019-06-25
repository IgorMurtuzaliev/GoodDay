using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IFileManager
    {
        Task<File> EditImage(User user, IFormFile file);
        Task<ICollection<File>> UploadMessagesFiles(int dialogId, int messageId, IFormFileCollection files);
        void DeleteDialogFiles(int dialogId);
        void DeleteUserAvatar(string path);
    }
}
