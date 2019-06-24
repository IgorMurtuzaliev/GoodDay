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
        bool Find(int? id, string userId);
        DeletedDialog Get(int? id, string userId);
        Task Edit(DeletedDialog item);
        DeletedDialog GetForMessageOutput(int? id, string userId);
        bool CheckForMessageOutput(int? id, string userId);
        bool Check(int? id);
        Task Save();
    }
}
