using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IFileRepository
    {
        Task Add(File item);
        Task Delete(int? id);
        Task Save();
    }
}
