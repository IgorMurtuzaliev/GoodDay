using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IFileRepository Files { get; }
        IMessageRepository Messages { get; }
        IRepository<Contact> Contacts { get; }
        IRepository<Dialog> Dialogs { get; }
        Task Save();
        void Dispose(bool disposing);
    }
}
