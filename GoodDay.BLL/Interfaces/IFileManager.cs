using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IFileManager
    {
        Task<File> EditImage(User user, IFormFile file);
        Task<File> UploadMessagesFiles(int dialogId, IFormFile file);
    }
}
