using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class FileManager : IFileManager
    {
        private IHostingEnvironment appenvironment;
        public IUnitOfWork unitOfWork;
        public FileManager(IHostingEnvironment _appenvironment, IUnitOfWork _unitOfWork)
        {
            appenvironment = _appenvironment;
            unitOfWork = _unitOfWork;
        }

        //public Task<File> AddProfileAvatar(User user)
        //{
        //    string path = "/Files/"+ user.File.;
        //    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
        //    {
        //        await file.CopyToAsync(fileStream);
        //    }
        //    FileModel newfile = new FileModel { Name = file.FileName, Path = path, ProductId = product.ProductId };
        //    context.Files.Add(newfile);

        //}

    }
}
