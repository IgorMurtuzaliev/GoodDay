using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Repositories
{
    public class DeletedDialogsRepository:IDeletedDialogsRepository
    {
        private readonly ApplicationDbContext dbContext;
        public DeletedDialogsRepository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Add(DeletedDialog deletedDialog)
        {
            await dbContext.DeletedDialogs.AddAsync(deletedDialog);
            await Save();
        }
        public void Delete(int? id)
        {
            DeletedDialog deletedDialog =  dbContext.DeletedDialogs.Single(c=>c.DialogId == id);
            if (deletedDialog != null)
                dbContext.DeletedDialogs.Remove(deletedDialog);
        }
        public bool Find(int? id, string userId)
        {
            return dbContext.DeletedDialogs.Any(c => c.DialogId == id && c.IsDeleted == true && c.DeleteByUserId == userId);
        }
        public async Task Edit(DeletedDialog item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
            await Save();
        }
        public bool Check(int? id)
        {
            return dbContext.DeletedDialogs.Any(c => c.DialogId == id);
        }
        public bool CheckForOutput(int? id, string userId)
        {
            return dbContext.DeletedDialogs.Any(c => c.DialogId == id && c.DeleteByUserId == userId);
        }
        public DeletedDialog Get(int? id, string userId)
        {
            return dbContext.DeletedDialogs.Single(c => c.DialogId == id && c.IsDeleted == true && c.DeleteByUserId == userId);
        }
        public DeletedDialog GetForOutput(int? id, string userId)
        {
            return dbContext.DeletedDialogs.Single(c => c.DialogId == id && c.DeleteByUserId == userId);
        }
        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
