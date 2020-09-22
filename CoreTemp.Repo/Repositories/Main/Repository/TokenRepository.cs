using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Repo.Repositories.Main.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Repo.Repositories.Main.Repository
{
    public class TokenRepository : Repository<MyToken>, ITokenRepository
    {
        private readonly DbContext _db;
        public TokenRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (CoreTempDbContext)dbContext;
        }
    }
}
