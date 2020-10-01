using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Repo.Repositories.Main.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Repo.Repositories.Main.Repository
{
    public class UserRepository : Repository<MyUser>, IUserRepository
    {
        private readonly DbContext _db;
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (CoreTempDbContext)dbContext;
        }

        public async Task<MyUser> GetUserByUserNameAsync(string username)
        {
            return await GetFirstOrDefaultAsync(p => p.UserName.Equals(username.ToLower()),null);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            if (await GetAsync(p => p.UserName.Equals(username.ToLower())) != null)
                return true;

            return false;
        }
    }
}
