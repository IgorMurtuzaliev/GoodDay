using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private UserRepository userRepository;
        private FileRepository fileRepository;
        private DialogRepository dialogRepository;
        private MessageRepository messageRepository;
        private ContactRepository contactRepository;

        public UnitOfWork(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(dbContext);
                return userRepository;
            }
        }

        public IRepository<Contact> Contacts
        {
            get
            {
                if (contactRepository == null)
                    contactRepository = new ContactRepository(dbContext);
                return contactRepository;
            }
        }

        public IRepository<Dialog> Dialogs
        {
            get
            {
                if (dialogRepository == null)
                    dialogRepository = new DialogRepository(dbContext);
                return dialogRepository;
            }
        }
        public IMessageRepository Messages
        {
            get
            {
                if (messageRepository == null)
                    messageRepository = new MessageRepository(dbContext);
                return messageRepository;
            }
        }
        public IFileRepository Files
        {
            get
            {
                if (fileRepository == null)
                    fileRepository = new FileRepository(dbContext);
                return fileRepository;
            }
        }
        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
