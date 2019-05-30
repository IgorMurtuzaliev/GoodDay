﻿using GoodDay.BLL.Interfaces;
using GoodDay.DAL.EF;
using GoodDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class SearchService : ISearchService
    {
        private ApplicationDbContext dbContext;
        public SearchService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<IEnumerable<User>> Search(string search)
        {
            return await dbContext.Users.Where(p => p.Email.Contains(search) || p.Phone.Contains(search)).ToListAsync();           
        }
    }
}
