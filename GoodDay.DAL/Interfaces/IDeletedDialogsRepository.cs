using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IDeletedDialogsRepository
    {
        Task Add(DeletedDialog deletedDialog);
        void Delete(int? id);
        bool Find(int? id);
        DeletedDialog Get(int? id);
        Task Save();
    }
}
