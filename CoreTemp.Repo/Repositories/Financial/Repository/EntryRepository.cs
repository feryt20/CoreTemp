using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.Models.Finantial;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Repo.Repositories.Financial.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Repo.Repositories.Financial.Repository
{
    public class EntryRepository : Repository<Entry>, IEntryRepository
    {
        private readonly DbContext _db;
        public EntryRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (FinantialDbContext)dbContext;
        }
    }
}
