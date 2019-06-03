using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace GoodDay.BLL.Services
{
    public class FileManager : IFileManager
    {
        public IUnitOfWork unitOfWork;
        public FileManager(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

    }
}
