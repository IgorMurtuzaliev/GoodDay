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
        IBlockRepository Blocks { get; }
        IContactRepository Contacts { get; }
        IDialogRepository Dialogs { get; }
        IDeletedDialogsRepository DeletedDialogs { get; }
        Task Save();
        void Dispose(bool disposing);
    }
}
